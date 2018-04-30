// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IDeleteCookiesCallback"/>.
    /// </summary>
    public class TaskDeleteCookiesCallback : IDeleteCookiesCallback
    {
        public const int InvalidNoOfCookiesDeleted = -1;

        private readonly TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
        private volatile bool isDisposed;
        private bool complete; //Only ever accessed on the same CEF thread, so no need for thread safety

        void IDeleteCookiesCallback.OnComplete(int numDeleted)
        {
            complete = true;

            taskCompletionSource.TrySetResultAsync(numDeleted);
        }

        public Task<int> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool IDeleteCookiesCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to InvalidNoOfCookiesDeleted
            if (complete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(InvalidNoOfCookiesDeleted);
            }

            isDisposed = true;
        }
    }
}
