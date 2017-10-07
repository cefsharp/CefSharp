// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

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
        private Task setResultTask;

        public TaskSetCookieCallback()
        {
            taskCompletionSource = new TaskCompletionSource<bool>();
            setResultTask = System.Threading.Tasks.Task.FromResult(false);
        }

        void ISetCookieCallback.OnComplete(bool success)
        {
            setResultTask = taskCompletionSource.TrySetResultAsync(success);
        }

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool ISetCookieCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        /// <summary>
        /// Task that can be awaited for the SetResult operation to complete - then you can check the Task property
        /// </summary>
        public Task SetResultTask
        {
            get { return  setResultTask; }
        }

        public void Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if (task.IsCompleted == false)
            {
                setResultTask = taskCompletionSource.TrySetResultAsync(false);
            }

            isDisposed = true;
        }
    }
}
