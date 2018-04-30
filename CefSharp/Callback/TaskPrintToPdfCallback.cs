// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public sealed class TaskPrintToPdfCallback : IPrintToPdfCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        private volatile bool isDisposed;
        private bool complete; //Only ever accessed on the same CEF thread, so no need for thread safety

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }    
        }

        void IPrintToPdfCallback.OnPdfPrintFinished(string path, bool ok)
        {
            complete = true;

            taskCompletionSource.TrySetResultAsync(ok);
        }

        bool IPrintToPdfCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if (complete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(false);
            }

            isDisposed = true;
        }
    }
}
