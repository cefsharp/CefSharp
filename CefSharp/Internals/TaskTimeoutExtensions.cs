// https://github.com/dotnet/runtime/blob/933988c35c172068652162adf6f20477231f815e/src/libraries/Common/tests/System/Threading/Tasks/TaskTimeoutExtensions.cs#L1
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/933988c35c172068652162adf6f20477231f815e/src/libraries/Common/tests/System/Threading/Tasks/TaskTimeoutExtensions.cs#L12

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// WaitAsync polyfills imported from .Net Runtime
    /// as we don't get access to this method in older .net versions
    /// </summary>
    public static class TaskTimeoutExtensions
    {
        public static Task<TResult> WaitAsync<TResult>(Task<TResult> task, int millisecondsTimeout) =>
            WaitAsync(task, TimeSpan.FromMilliseconds(millisecondsTimeout), default);

        public static Task<TResult> WaitAsync<TResult>(Task<TResult> task, TimeSpan timeout) =>
            WaitAsync(task, timeout, default);

        public static Task<TResult> WaitAsync<TResult>(Task<TResult> task, CancellationToken cancellationToken) =>
            WaitAsync(task, Timeout.InfiniteTimeSpan, cancellationToken);

        public static async Task<TResult> WaitAsync<TResult>(Task<TResult> task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            using (new Timer(s => ((TaskCompletionSource<TResult>)s).TrySetException(new TimeoutException()), tcs, timeout, Timeout.InfiniteTimeSpan))
            using (cancellationToken.Register(s => ((TaskCompletionSource<TResult>)s).TrySetCanceled(), tcs))
            {
                return await (await Task.WhenAny(task, tcs.Task).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }
    }
}
