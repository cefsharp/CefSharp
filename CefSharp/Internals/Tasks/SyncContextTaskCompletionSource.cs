// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals.Tasks
{
    /// <summary>
    /// TaskCompletionSource that executes it's continuation on the captured
    /// <see cref="SynchronizationContext"/>. If <see cref="SyncContext"/> is null.
    /// then the current **executing** thread will be called. e.g. The thread that
    /// called <see cref="TaskCompletionSource{TResult}.TrySetResult(TResult)"/>
    /// (or other Set/Try set methods).
    /// </summary>
    /// <typeparam name="TResult">Result Type</typeparam>
    public class SyncContextTaskCompletionSource<TResult> : TaskCompletionSource<TResult>
    {
        /// <summary>
        /// Captured Sync Context
        /// </summary>
        public SynchronizationContext SyncContext { get; set; }
    }
}
