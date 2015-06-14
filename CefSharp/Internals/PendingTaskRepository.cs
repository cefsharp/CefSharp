using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals.Messaging
{
    public sealed class PendingTaskRepository<TResult>
    {
        private readonly ConcurrentDictionary<long, TaskCompletionSource<TResult>> pendingTasks =
            new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();
        private long lastId = 0;

        public long CreatePendingTask(out TaskCompletionSource<TResult> completionSource)
        {
            completionSource = new TaskCompletionSource<TResult>();
            return SaveCompletionSource(completionSource);
        }

        public long CreatePendingTaskWithTimeout(out TaskCompletionSource<TResult> completionSource, TimeSpan timeout)
        {
            completionSource = new TaskCompletionSource<TResult>();
            var id = SaveCompletionSource(completionSource);
            Timer timer = null;
            timer = new Timer(state =>
            {
                timer.Dispose();
                RemovePendingTask(id);
                ((TaskCompletionSource<TResult>)state).TrySetCanceled(); 
            }, completionSource, timeout, TimeSpan.FromMilliseconds(-1));

            return id;
        }

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
