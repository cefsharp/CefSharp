// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public interface IPendingTaskRepository<TResult>
    {
        KeyValuePair<long, TaskCompletionSource<TResult>> CreatePendingTask(TimeSpan? timeout = null);
        TaskCompletionSource<TResult> RemovePendingTask(long id);
    }
}
