// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IGetGeolocationCallback"/> for use with geo positioning
    /// </summary>
    public class TaskGetGeolocationCallback : IGetGeolocationCallback
    {
        private readonly TaskCompletionSource<Geoposition> taskCompletionSource;
        private bool hasData;
        private volatile bool isDisposed;

        public TaskGetGeolocationCallback()
        {
            taskCompletionSource = new TaskCompletionSource<Geoposition>();
        }

        void IGetGeolocationCallback.OnLocationUpdate(Geoposition position)
        {
            taskCompletionSource.TrySetResultAsync(position);

            hasData = true;
        }

        bool IGetGeolocationCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        public Task<Geoposition> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            if (hasData == false)
            {
                //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                taskCompletionSource.TrySetResultAsync(null);
            }

            isDisposed = true;
        }		
    }
}
