// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Provides a callback implementation of <see cref="IRegisterCdmCallback"/> for use with asynchronous Widevine CDM registration.
    /// </summary>
    public class TaskRegisterCdmCallback : IRegisterCdmCallback
    {
        private readonly TaskCompletionSource<CdmRegistration> taskCompletionSource;
        private volatile bool isDisposed;
        private bool onComplete; //Only ever accessed on the same CEF thread, so no need for thread safety

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskRegisterCdmCallback()
        {
            taskCompletionSource = new TaskCompletionSource<CdmRegistration>();
        }

        void IRegisterCdmCallback.OnRegistrationComplete(CdmRegistration registration)
        {
            onComplete = true;

            taskCompletionSource.TrySetResultAsync(registration);
        }

        /// <summary>
        /// Task used to await this callback
        /// </summary>
        public Task<CdmRegistration> Task
        {
            get { return taskCompletionSource.Task; }
        }

        bool IRegisterCdmCallback.IsDisposed
        {
            get { return isDisposed; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If onComplete is false then IRegisterCdmCallback.OnRegistrationComplete was never called,
            //so we'll set the result to false. Calling TrySetResultAsync multiple times 
            //can result in the issue outlined in https://github.com/cefsharp/CefSharp/pull/2349
            if (onComplete == false && task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(null);
            }

            isDisposed = true;
        }
    }
}
