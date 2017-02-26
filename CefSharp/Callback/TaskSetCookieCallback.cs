// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
    public class TaskSetCookieCallback : ISetCookieCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;
        private volatile bool isDisposed;

        public TaskSetCookieCallback()
        {
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        void ISetCookieCallback.OnComplete(bool success)
        {
            taskCompletionSource.TrySetResultAsync(success);
        }

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

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if (task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(false);
            }

            isDisposed = true;

        }
    }
}
