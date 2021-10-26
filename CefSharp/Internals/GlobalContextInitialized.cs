// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public static class GlobalContextInitialized
    {
        private static TaskCompletionSource<bool> TaskCompletionSource = new TaskCompletionSource<bool>();

        public static Task<bool> Task
        {
            get { return TaskCompletionSource.Task; }
        }

        public static void SetResult(bool success)
        {
            TaskCompletionSource.TrySetResult(success);
        }

        public static void SetException(Exception ex)
        {
            TaskCompletionSource.TrySetException(ex);
        }

        /// <summary>
        /// We need to be sure the CEF Global Context has been initialized before
        /// we create the browser. If the CefRequestContext has already been initialzed
        /// then we'll execute syncroniously. If the CefRequestContext hasn't been
        /// initialized then we will continue on the CEF UI Thread.
        /// https://github.com/cefsharp/CefSharp/issues/3850
        /// </summary>
        /// <param name="action">action to invoke</param>
        public static void ExecuteOrEnqueue(Action<bool> action)
        {
            TaskCompletionSource.Task.ContinueWith((t) =>
            {
                action(t.Result);

            }, TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}
