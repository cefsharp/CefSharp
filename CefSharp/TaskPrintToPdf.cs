// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;

namespace CefSharp
{
    public sealed class TaskPrintToPdf : IPrintToPdfCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }    
        }

        public void OnPdfPrintFinished(string path, bool ok)
        {
            taskCompletionSource.TrySetResult(ok);
        }
    }
}
