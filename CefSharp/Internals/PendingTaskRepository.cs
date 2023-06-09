// Copyright © 2015 The CefSharp Authors. All rights reserved.
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
    /// Class to store TaskCompletionSources indexed by a unique id. There are two distinct ConcurrentDictionary
    /// instances as we have some Tasks that are created from the browser process (EvaluateScriptAsync) calls, and
    /// some that are created for <see cref="IJavascriptCallback"/> instances for which the Id's are created
    /// in the render process. 
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the tasks held.</typeparam>
    public sealed class PendingTaskRepository<TResult>
    {
        private readonly ConcurrentDictionary<long, TaskCompletionSource<TResult>> pendingTasks =
            new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();
        private readonly ConcurrentDictionary<long, TaskCompletionSource<TResult>> callbackPendingTasks =
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
            var taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var id = Interlocked.Increment(ref lastId);
            pendingTasks.TryAdd(id, taskCompletionSource);

            if (timeout.HasValue)
            {
                taskCompletionSource = taskCompletionSource.WithTimeout(timeout.Value, () => RemovePendingTask(id));
            }

            return new KeyValuePair<long, TaskCompletionSource<TResult>>(id, taskCompletionSource);
        }

        /// <summary>
        /// Creates a new pending task with a timeout.
        /// </summary>
        /// <param name="id">Id passed in from the render process</param>
        /// <param name="timeout">The maximum running time of the task.</param>
        /// <returns>The unique id of the newly created pending task and the newly created <see cref="TaskCompletionSource{TResult}"/>.</returns>
        public KeyValuePair<long, TaskCompletionSource<TResult>> CreateJavascriptCallbackPendingTask(long id, TimeSpan? timeout = null)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            callbackPendingTasks.TryAdd(id, taskCompletionSource);

            if (timeout.HasValue)
            {
                taskCompletionSource = taskCompletionSource.WithTimeout(timeout.Value, () => RemoveJavascriptCallbackPendingTask(id));
            }

            return new KeyValuePair<long, TaskCompletionSource<TResult>>(id, taskCompletionSource);
        }

        /// <summary>
        /// If a <see cref="TaskCompletionSource{TResult}"/> is found matching <paramref name="id"/>
        /// then it is removed from the ConcurrentDictionary and returned.
        /// </summary>
        /// <param name="id">Unique id of the pending task.</param>
        /// <returns>
        /// The <see cref="TaskCompletionSource{TResult}"/> associated with the given id
        /// or null if no matching TaskComplectionSource found.
        /// </returns>
        public TaskCompletionSource<TResult> RemovePendingTask(long id)
        {
            TaskCompletionSource<TResult> result;
            if (pendingTasks.TryRemove(id, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// If a <see cref="TaskCompletionSource{TResult}"/> is found matching <paramref name="id"/>
        /// then it is removed from the ConcurrentDictionary and returned.
        /// </summary>
        /// <param name="id">Unique id of the pending task.</param>
        /// <returns>
        /// The <see cref="TaskCompletionSource{TResult}"/> associated with the given id
        /// or null if no matching TaskComplectionSource found.
        /// </returns>
        public TaskCompletionSource<TResult> RemoveJavascriptCallbackPendingTask(long id)
        {
            TaskCompletionSource<TResult> result;

            if (callbackPendingTasks.TryRemove(id, out result))
            {
                return result;
            }

            return null;
        }
    }
}
