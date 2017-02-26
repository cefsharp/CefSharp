// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskCompletionCallback : ICompletionCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;

        public TaskCompletionCallback()
        {
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        void ICompletionCallback.OnComplete()
        {
            taskCompletionSource.TrySetResultAsync(true);
        }

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if(task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(false);
            }
        }
    }
}
