// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IPrintToPdfCallback"/>.
    /// </summary>
    public sealed class TaskPrintToPdfCallback : IPrintToPdfCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Task used to await this callback
        /// </summary>
        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IPrintToPdfCallback.OnPdfPrintFinished(string path, bool ok)
        {
            onComplete = true;

            taskCompletionSource.TrySetResultAsync(ok);
        }

        bool IPrintToPdfCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If onComplete is false then IPrintToPdfCallback.OnPdfPrintFinished was never called,
            //so we'll set the result to false. Calling TrySetResultAsync multiple times 
            //can result in the issue outlined in https://github.com/cefsharp/CefSharp/pull/2349
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(false);
            }

            isDisposed = true;
        }
    }
}
