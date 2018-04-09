// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Wraps <see cref="TaskCompletionSource{T}"/> by providing a way to set its result in an asynchronous
    /// fashion. This prevents the resulting Task continuations from being executed synchronously on the same
    /// thread as the one completing the Task, which may be better when that thread is a CEF UI thread.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncTaskCompletionSource<T>
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        private int resultSet = 0;

        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> created by the wrapped <see cref="AsyncTaskCompletionSource{T}"/> instance.
        /// </summary>
        /// <seealso cref="TaskCompletionSource{TResult}.Task"/>
        public Task<T> Task => tcs.Task;

        /// <summary>
        /// Sets the wrapped <see cref="TaskCompletionSource{T}"/> instance result in an asynchronous fashion.
        /// This prevents the Task continuations from being executed synchronously on the same thread as the
        /// one calling this method, which is required when this thread is a CEF UI thread.
        /// </summary>
        /// <param name="result">result</param>
        public void TrySetResultAsync(T result)
        {
            if (Interlocked.Exchange(ref resultSet, 1) == 0)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => tcs.TrySetResult(result),
                    CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }
        }
    }
}
