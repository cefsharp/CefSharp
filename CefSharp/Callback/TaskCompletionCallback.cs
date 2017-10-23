// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskCompletionCallback : ICompletionCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;
        private Task setResultTask;

        public TaskCompletionCallback()
        {
            taskCompletionSource = new TaskCompletionSource<bool>();
            setResultTask = System.Threading.Tasks.Task.FromResult(false);
        }

        void ICompletionCallback.OnComplete()
        {
            setResultTask = taskCompletionSource.TrySetResultAsync(true);
        }

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }

        public void Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if(task.IsCompleted == false)
            {
                setResultTask =  taskCompletionSource.TrySetResultAsync(false);
            }
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
