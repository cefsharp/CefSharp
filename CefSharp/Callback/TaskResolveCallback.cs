// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskResolveCallback : IResolveCallback
    {
        private readonly TaskCompletionSource<ResolveCallbackResult> taskCompletionSource;

        public TaskResolveCallback()
        {
            taskCompletionSource = new TaskCompletionSource<ResolveCallbackResult>();
        }

        public void OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses)
        {
            taskCompletionSource.TrySetResultAsync(new ResolveCallbackResult(result, resolvedIpAddresses));
        }

        public Task<ResolveCallbackResult> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}
