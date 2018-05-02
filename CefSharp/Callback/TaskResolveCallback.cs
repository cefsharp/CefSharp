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
        private readonly TaskCompletionSource<ResolveCallbackResult> taskCompletionSource = new TaskCompletionSource<ResolveCallbackResult>();
        private volatile bool isDisposed;
        private bool complete; //Only ever accessed on the same CEF thread, so no need for thread safety

        void IResolveCallback.OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses)
        {
            complete = true;

            taskCompletionSource.TrySetResultAsync(new ResolveCallbackResult(result, resolvedIpAddresses));
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to null
            if (complete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(new ResolveCallbackResult(CefErrorCode.Unexpected, null));
            }

            isDisposed = true;
        }

        public Task<ResolveCallbackResult> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool IResolveCallback.IsDisposed
        {
            get { return isDisposed; }
        }
    }
}
