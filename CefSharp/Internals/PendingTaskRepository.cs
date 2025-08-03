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
        private readonly ConcurrentDictionary<string, FramePendingTaskRepository<TResult>> framePendingTasks =
            new ConcurrentDictionary<string, FramePendingTaskRepository<TResult>>();
        //should only be accessed by Interlocked.Increment
        private long lastId;

        /// <summary>
        /// Creates a new pending task with a timeout.
        /// </summary>
        /// <param name="frameId">The frame id in which the task is created.</param>
        /// <param name="timeout">The maximum running time of the task.</param>
        /// <returns>The unique id of the newly created pending task and the newly created <see cref="TaskCompletionSource{TResult}"/>.</returns>
        public KeyValuePair<long, TaskCompletionSource<TResult>> CreatePendingTask(string frameId, TimeSpan? timeout = null)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var id = Interlocked.Increment(ref lastId);
            var result = new KeyValuePair<long, TaskCompletionSource<TResult>>(id, taskCompletionSource);
#if NETCOREAPP
            framePendingTasks.AddOrUpdate(
                frameId,
                (key, state) => { var value = new FramePendingTaskRepository<TResult>(); value.PendingTasks.TryAdd(state.Key, state.Value); return value; },
                (key, value, state) => { value.PendingTasks.TryAdd(state.Key, state.Value); return value; },
                result
            );
#else
            framePendingTasks.AddOrUpdate(
                frameId,
                (key) => { var value = new FramePendingTaskRepository(); value.PendingTasks.TryAdd(id, taskCompletionSource); return value; },
                (key, value) => { value.PendingTasks.TryAdd(id, taskCompletionSource); return value; }
            );
#endif

            if (timeout.HasValue)
            {
                taskCompletionSource.WithTimeout(timeout.Value, () => RemovePendingTask(frameId, id));
            }

            return result;
        }

        /// <summary>
        /// Creates a new pending task with a timeout.
        /// </summary>
        /// <param name="frameId">The frame id in which the task is created.</param>
        /// <param name="id">Id passed in from the render process</param>
        /// <param name="timeout">The maximum running time of the task.</param>
        /// <returns>The unique id of the newly created pending task and the newly created <see cref="TaskCompletionSource{TResult}"/>.</returns>
        public KeyValuePair<long, TaskCompletionSource<TResult>> CreateJavascriptCallbackPendingTask(string frameId, long id, TimeSpan? timeout = null)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var result = new KeyValuePair<long, TaskCompletionSource<TResult>>(id, taskCompletionSource);
#if NETCOREAPP
            framePendingTasks.AddOrUpdate(
                frameId,
                (key, state) => { var value = new FramePendingTaskRepository<TResult>(); value.CallbackPendingTasks.TryAdd(state.Key, state.Value); return value; },
                (key, value, state) => { value.CallbackPendingTasks.TryAdd(state.Key, state.Value); return value; },
                result
            );
#else
            framePendingTasks.AddOrUpdate(
                frameId,
                (key) => { var value = new FramePendingTaskRepository(); value.CallbackPendingTasks.TryAdd(id, taskCompletionSource); return value; },
                (key, value) => { value.CallbackPendingTasks.TryAdd(id, taskCompletionSource); return value; }
            );
#endif

            if (timeout.HasValue)
            {
                taskCompletionSource.WithTimeout(timeout.Value, () => RemoveJavascriptCallbackPendingTask(frameId, id));
            }

            return result;
        }

        /// <summary>
        /// If a <see cref="TaskCompletionSource{TResult}"/> is found matching <paramref name="id"/>
        /// then it is removed from the ConcurrentDictionary and returned.
        /// </summary>
        /// <param name="frameId">The frame id.</param>
        /// <param name="id">Unique id of the pending task.</param>
        /// <returns>
        /// The <see cref="TaskCompletionSource{TResult}"/> associated with the given id
        /// or null if no matching TaskComplectionSource found.
        /// </returns>
        public TaskCompletionSource<TResult> RemovePendingTask(string frameId, long id)
        {
            FramePendingTaskRepository<TResult> repository;
            if (framePendingTasks.TryGetValue(frameId, out repository))
            {
                TaskCompletionSource<TResult> result;
                if (repository.PendingTasks.TryRemove(id, out result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// If a <see cref="TaskCompletionSource{TResult}"/> is found matching <paramref name="id"/>
        /// then it is removed from the ConcurrentDictionary and returned.
        /// </summary>
        /// <param name="frameId">The frame id.</param>
        /// <param name="id">Unique id of the pending task.</param>
        /// <returns>
        /// The <see cref="TaskCompletionSource{TResult}"/> associated with the given id
        /// or null if no matching TaskComplectionSource found.
        /// </returns>
        public TaskCompletionSource<TResult> RemoveJavascriptCallbackPendingTask(string frameId, long id)
        {
            FramePendingTaskRepository<TResult> repository;
            if (framePendingTasks.TryGetValue(frameId, out repository))
            {
                TaskCompletionSource<TResult> result;
                if (repository.CallbackPendingTasks.TryRemove(id, out result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Cancels all pending tasks of a frame.
        /// </summary>
        /// <param name="frameId">The frame id.</param>
        public void CancelPendingTasks(string frameId)
        {
            FramePendingTaskRepository<TResult> repository;
            if (framePendingTasks.TryRemove(frameId, out repository))
            {
                repository.Dispose();
            }
        }

        /// <summary>
        /// Cancels all pending tasks.
        /// </summary>
        public void CancelPendingTasks()
        {
            foreach (var repository in framePendingTasks.Values)
            {
                repository.Dispose();
            }
            framePendingTasks.Clear();
        }
    }
}
