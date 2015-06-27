// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals.Messaging
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
        private long lastId = 0;

        /// <summary>
        /// Creates a new pending task.
        /// </summary>
        /// <param name="completionSource">The newly created <see cref="TaskCompletionSource{TResult}"/></param>
        /// <returns>The unique id of the newly created pending task.</returns>
        public long CreatePendingTask(out TaskCompletionSource<TResult> completionSource)
        {
            completionSource = new TaskCompletionSource<TResult>();
            return SaveCompletionSource(completionSource);
        }

        /// <summary>
        /// Creates a new pending task with a timeout.
        /// </summary>
        /// <param name="timeout">The maximum running time of the task.</param>
        /// <param name="completionSource">The newly created <see cref="TaskCompletionSource{TResult}"/></param>
        /// <returns>The unique id of the newly created pending task.</returns>
        public long CreatePendingTaskWithTimeout(out TaskCompletionSource<TResult> completionSource, TimeSpan timeout)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>();
            completionSource = taskCompletionSource;

            var id = SaveCompletionSource(completionSource);
            Timer timer = null;
            timer = new Timer(state =>
            {
                timer.Dispose();
                RemovePendingTask(id);
                taskCompletionSource.TrySetCanceled();
            }, null, timeout, TimeSpan.FromMilliseconds(-1));

            return id;
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

        private long SaveCompletionSource(TaskCompletionSource<TResult> completionSource)
        {
            var id = Interlocked.Increment(ref lastId);
            pendingTasks.TryAdd(id, completionSource);
            return id;
        }
    }
}