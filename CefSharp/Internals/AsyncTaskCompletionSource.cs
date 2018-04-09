using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Wraps <see cref="TaskCompletionSource{T}"/> by providing a way to set its result in an async fashion.
    /// This prevents the Task Continuation being executed sync on the same thread, which may be required when
    /// continuations must not run on CEF UI threads.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncTaskCompletionSource<T>
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        private int resultSet = 0;

        /// <summary>
        /// Gets the <see cref="Task{TResult}"/> created by this <see cref="AsyncTaskCompletionSource{T}"/>
        /// </summary>
        /// <seealso cref="TaskCompletionSource{TResult}.Task"/>
        public Task<T> Task => tcs.Task;

        /// <summary>
        /// Set the TaskCompletionSource in an async fashion. This prevents the Task Continuation being executed sync on the same thread
        /// This is required otherwise contintinuations will happen on CEF UI threads
        /// </summary>
        /// <param name="result">result</param>
        public void TrySetResultAsync(T result)
        {
            if (Interlocked.Exchange(ref resultSet, 1) == 0)
                System.Threading.Tasks.Task.Factory.StartNew(() => tcs.TrySetResult(result), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }
}
