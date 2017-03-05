// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
    public class TaskRegisterCdmCallback: IRegisterCdmCallback
    {
        private readonly TaskCompletionSource<CdmRegistration> taskCompletionSource;

        public TaskRegisterCdmCallback()
        {
            taskCompletionSource = new TaskCompletionSource<CdmRegistration>();
        }

        void IRegisterCdmCallback.OnRegistrationComplete(CdmRegistration registration)
        {
            taskCompletionSource.TrySetResultAsync(registration);
        }

        public Task<CdmRegistration> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            var task = taskCompletionSource.Task;

            //If the Task hasn't completed and this is being disposed then
            //set the TCS to false
            if (task.IsCompleted == false)
            {
                taskCompletionSource.TrySetResultAsync(null);
            }
        }
    }
}
