// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="ISetCookieCallback"/>.
    /// </summary>
    public sealed class TaskSetCookieCallback : ISetCookieCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Initializes a new instance of the TaskSetCookieCallback class.
        /// </summary>
        public TaskSetCookieCallback()
        {
            taskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        void ISetCookieCallback.OnComplete(bool success)
        {
            onComplete = true;

            taskCompletionSource.TrySetResult(success);
        }

        /// <summary>
        /// Task used to await this callback
        /// </summary>
        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool ISetCookieCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If onComplete is false then ISetCookieCallback.OnComplete was never called,
            //so we'll set the result to false. 
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResult(false);
            }

            isDisposed = true;
        }
    }
}
