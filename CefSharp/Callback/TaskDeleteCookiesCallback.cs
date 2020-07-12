// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IDeleteCookiesCallback"/>.
    /// </summary>
    public class TaskDeleteCookiesCallback : IDeleteCookiesCallback
    {
        /// <summary>
        /// Invalid Number of Cookies
        /// </summary>
        public const int InvalidNoOfCookiesDeleted = -1;

        private readonly TaskCompletionSource<int> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskDeleteCookiesCallback()
        {
            taskCompletionSource = new TaskCompletionSource<int>();
        }

        void IDeleteCookiesCallback.OnComplete(int numDeleted)
        {
            onComplete = true;

            taskCompletionSource.TrySetResultAsync(numDeleted);
        }

        /// <summary>
        /// Task used to await this callback
        /// </summary>
        public Task<int> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool IDeleteCookiesCallback.IsDisposed
        {
            get { return isDisposed; }
        }
        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If onComplete is false then IDeleteCookiesCallback.OnComplete was never called,
            //so we'll set the result to false. Calling TrySetResultAsync multiple times 
            //can result in the issue outlined in https://github.com/cefsharp/CefSharp/pull/2349
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(InvalidNoOfCookiesDeleted);
            }

            isDisposed = true;
        }
    }
}
