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

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }    
        }

        void IPrintToPdfCallback.OnPdfPrintFinished(string path, bool ok)
        {
            taskCompletionSource.TrySetResultAsync(ok);
        }

        void IDisposable.Dispose()
        {
            //TODO: Check if this is ever called
        }
    }
}
