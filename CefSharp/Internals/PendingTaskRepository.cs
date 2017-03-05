// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// Class to store TaskCompletionSources indexed by a unique id.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the tasks held.</typeparam>
    public sealed class PendingTaskRepository<TResult>
    {
        private readonly ConcurrentDictionary<long, TaskCompletionSource<TResult>> pendingTasks =
            new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();
        //should only be accessed by Interlocked.Increment
        private long lastId;

        /// <summary>
        /// Creates a new pending task with a timeout.
        /// </summary>
        /// <param name="timeout">The maximum running time of the task.</param>
        /// <returns>The unique id of the newly created pending task and the newly created <see cref="TaskCompletionSource{TResult}"/>.</returns>
        public KeyValuePair<long, TaskCompletionSource<TResult>> CreatePendingTask(TimeSpan? timeout = null)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>();

            var id = Interlocked.Increment(ref lastId);
            pendingTasks.TryAdd(id, taskCompletionSource);

            if (timeout.HasValue)
            {
                taskCompletionSource = taskCompletionSource.WithTimeout(timeout.Value, () => RemovePendingTask(id));
            }

            return new KeyValuePair<long, TaskCompletionSource<TResult>>(id, taskCompletionSource);
        }

        /// <summary>
        /// Gets and removed pending task by id.
        /// </summary>
        /// <param name="id">Unique id of the pending task.</param>
        /// <returns>
        /// The <see cref="TaskCompletionSource{TResult}"/> associated with the given id.
        /// </returns>
        public TaskCompletionSource<TResult> RemovePendingTask(long id)
        {
            TaskCompletionSource<TResult> result;
            pendingTasks.TryRemove(id, out result);
            return result;
        }
    }
}