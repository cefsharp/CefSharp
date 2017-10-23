// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        private readonly TaskCompletionSource<int> taskCompletionSource;
        private volatile bool isDisposed;
        private Task setResultTask;

        public TaskDeleteCookiesCallback()
        {
            taskCompletionSource = new TaskCompletionSource<int>();
            setResultTask = System.Threading.Tasks.Task.FromResult(false);
        }

        void IDeleteCookiesCallback.OnComplete(int numDeleted)
        {
            setResultTask = taskCompletionSource.TrySetResultAsync(numDeleted);
        }

        public Task<int> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool IDeleteCookiesCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        public void Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if (task.IsCompleted == false)
            {
                setResultTask = taskCompletionSource.TrySetResultAsync(-1);
            }

            isDisposed = true;
        }

        /// <summary>
        /// Task that can be awaited for the SetResult operation to complete - then you can check the Task property
        /// </summary>
        public Task SetResultTask
        {
            get { return setResultTask; }
        }
    }
}
