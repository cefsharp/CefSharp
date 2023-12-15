// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// TaskExtension based on the following
    /// https://github.com/ChadBurggraf/parallel-extensions-extras/blob/master/Extensions/TaskExtrasExtensions.cs
    /// https://github.com/ChadBurggraf/parallel-extensions-extras/blob/ec803e58eee28c698e44f55f49c5ad6671b1aa58/Extensions/TaskCompletionSourceExtensions.cs
    /// </summary>
    public static class TaskExtensions
    {
        public static TaskCompletionSource<TResult> WithTimeout<TResult>(this TaskCompletionSource<TResult> taskCompletionSource, TimeSpan timeout, Action cancelled)
        {
            var timer = new Timer(state =>
            {
                ((Timer)state).Dispose();
                if (taskCompletionSource.Task.Status != TaskStatus.RanToCompletion)
                {
                    taskCompletionSource.TrySetCanceled();
                    if (cancelled != null)
                    {
                        cancelled();
                    }
                }
            });
            timer.Change(timeout, Timeout.InfiniteTimeSpan);

            return taskCompletionSource;
        }
    }
}
