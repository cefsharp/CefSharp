// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IResolveCallback"/>.
    /// </summary>
    public class TaskResolveCallback : IResolveCallback
    {
        private readonly TaskCompletionSource<ResolveCallbackResult> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Default constructor
        /// </summary>
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

        /// <summary>
        /// Task used to await this callback
        /// </summary>
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
