// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    internal sealed class FramePendingTaskRepository<TResult> : IDisposable
    {
        public ConcurrentDictionary<long, TaskCompletionSource<TResult>> PendingTasks { get; } =
            new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();
        public ConcurrentDictionary<long, TaskCompletionSource<TResult>> CallbackPendingTasks { get; } =
            new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();

        public void Dispose()
        {
            foreach (var tcs in PendingTasks.Values)
            {
                tcs.TrySetCanceled();
            }
            PendingTasks.Clear();

            foreach (var tcs in CallbackPendingTasks.Values)
            {
                tcs.TrySetCanceled();
            }
            CallbackPendingTasks.Clear();
        }
    }
}
