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
    public static class CefThread
    {
        /// <summary>
        /// TaskFactory will be null before Cef.Initialize is called
        /// and null after Cef.Shutdown is called.
        /// </summary>
        public static TaskFactory UiThreadTaskFactory { get; set; }

        public static Func<bool> CurrentOnUiThreadDelegate { get; set; }

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

        public static bool CurrentlyOnUiThread
        {
            get
            {
                if (CurrentOnUiThreadDelegate == null)
                {
                    return false;
                }

                return CurrentOnUiThreadDelegate();
            }
        }

        public static Task<TResult> ExecuteOnUiThread<TResult>(Func<TResult> function)
        {
            var taskFactory = UiThreadTaskFactory;

            if (taskFactory == null)
            {
                throw new Exception("CefThread.UiThreadTaskFactory is null, ");
            }

            return taskFactory.StartNew(function);
        }
    }
}
