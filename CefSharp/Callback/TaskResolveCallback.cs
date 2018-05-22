// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskResolveCallback : IResolveCallback
    {
        private readonly TaskCompletionSource<ResolveCallbackResult> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        public TaskResolveCallback()
        {
            taskCompletionSource = new TaskCompletionSource<ResolveCallbackResult>();
        }

        void IResolveCallback.OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses)
        {
            onComplete = true;

            taskCompletionSource.TrySetResultAsync(new ResolveCallbackResult(result, resolvedIpAddresses));
        }

        bool IResolveCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        public Task<ResolveCallbackResult> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If onComplete is false then IResolveCallback.OnResolveCompleted was never called,
            //so we'll set the result to false. Calling TrySetResultAsync multiple times 
            //can result in the issue outlined in https://github.com/cefsharp/CefSharp/pull/2349
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(new ResolveCallbackResult(CefErrorCode.Unexpected, null));
            }

            isDisposed = true;
        }
    }
}
