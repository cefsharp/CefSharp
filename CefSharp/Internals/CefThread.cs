// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// To access the CEF threads we expose a TaskFactory, as this requires managed vc++ this
    /// exists in CefSharp.Core it cannot be directly accessed in CefSharp.dll. When
    /// Cef.Initialized is called we pass a reference to the TaskFactory here so we
    /// can write methods (typically extension methods) in this assembly.
    /// </summary>
    /// TODO: This can likely be removed and code that depends on this can be moved
    /// to CefSharp.Core and interact directly with the C++ api
    public static class CefThread
    {
        private static readonly object LockObj = new object();

        /// <summary>
        /// TaskFactory will be null before Cef.Initialize is called
        /// and null after Cef.Shutdown is called.
        /// </summary>
        public static TaskFactory UiThreadTaskFactory { get; private set; }

        /// <summary>
        /// Event fired after Cef.Initialze has been called, we can now start
        /// posting Tasks to the CEF UI Thread.
        /// </summary>
        public static event EventHandler Initialized;

        /// <summary>
        /// Delegate used to wrap the native call to CefCurrentlyOn(CefThreadId::TID_UI).
        /// </summary>
        public static Func<bool> CurrentOnUiThreadDelegate { get; private set; }

        /// <summary>
        /// true if we have a reference to the UiThreadTaskFactory
        /// TaskFactory, otherwise false
        /// </summary>
        /// <remarks>
        /// The current implementation isn't thread safe, generally speaking this shouldn't be a problem
        /// </remarks>
        public static bool CanExecuteOnUiThread
        {
            get
            {
                return UiThreadTaskFactory != null;
            }
        }

        /// <summary>
        /// Currently on the CEF UI Thread
        /// </summary>
        public static bool CurrentlyOnUiThread
        {
            get
            {
                var d = CurrentOnUiThreadDelegate;
                if (d == null)
                {
                    return false;
                }

                return d();
            }
        }

        /// <summary>
        /// returns true if Cef.Shutdown been called, otherwise false.
        /// </summary>
        public static bool HasShutdown { get; private set; }

        /// <summary>
        /// Execute the provided function on the CEF UI Thread
        /// </summary>
        /// <typeparam name="TResult">result</typeparam>
        /// <param name="function">function</param>
        /// <returns>Task{Result}</returns>
        public static Task<TResult> ExecuteOnUiThread<TResult>(Func<TResult> function)
        {
            lock (LockObj)
            {
                if (HasShutdown)
                {
                    throw new Exception("Cef.Shutdown has already been called, it's no longer possible to execute on the CEF UI Thread. Check CefThread.HasShutdown to guard against this execption");
                }

                var taskFactory = UiThreadTaskFactory;

                if (taskFactory == null)
                {
                    //We don't have a task factory yet, so we'll queue for execution.
                    return QueueForExcutionWhenUiThreadCreated(function);
                }

                return taskFactory.StartNew(function);
            }
        }

        /// <summary>
        /// Wait for CEF to Initialize, continuation happens on
        /// the CEF UI Thraed.
        /// </summary>
        /// <returns>Task that can be awaited</returns>
        private static Task<T> QueueForExcutionWhenUiThreadCreated<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();

            EventHandler handler = null;

            handler = (s, args) =>
            {
                Initialized -= handler;

                var result = func();

                tcs.SetResult(result);
            };

            Initialized += handler;

            return tcs.Task;
        }

        /// <summary>
        /// Called when the CEF UI Thread is a
        /// </summary>
        public static void Initialize(TaskFactory uiThreadTaskFactory, Func<bool> currentOnUiThreadDelegate)
        {
            lock (LockObj)
            {
                Initialized?.Invoke(null, EventArgs.Empty);

                UiThreadTaskFactory = uiThreadTaskFactory;
                CurrentOnUiThreadDelegate = currentOnUiThreadDelegate;
            }
        }

        /// <summary>
        /// !!WARNING!! DO NOT CALL THIS YOURSELF, THIS WILL BE CALLED INTERNALLY.
        /// Called when Cef.Shutdown is called to cleanup our references
        /// and release any event handlers.
        /// </summary>
        public static void Shutdown()
        {
            lock (LockObj)
            {
                CurrentOnUiThreadDelegate = null;
                Initialized = null;
                UiThreadTaskFactory = null;
                HasShutdown = true;
            }
        }
    }
}
