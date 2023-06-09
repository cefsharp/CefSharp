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
    public sealed class TaskResolveCallback : IResolveCallback
    {
        private readonly TaskCompletionSource<ResolveCallbackResult> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Initializes a new instance of the TaskResolveCallback class.
        /// </summary>
        public TaskResolveCallback()
        {
            taskCompletionSource = new TaskCompletionSource<ResolveCallbackResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        void IResolveCallback.OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses)
        {
            onComplete = true;

            taskCompletionSource.TrySetResult(new ResolveCallbackResult(result, resolvedIpAddresses));
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
            //so we'll set the result to false. 
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResult(new ResolveCallbackResult(CefErrorCode.Unexpected, null));
            }

            isDisposed = true;
        }
    }
}
