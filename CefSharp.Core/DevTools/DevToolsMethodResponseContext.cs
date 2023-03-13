// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading;

namespace CefSharp.DevTools
{
    // Helper class for setting the method response in the correct sync context
    internal struct DevToolsMethodResponseContext
    {
        public readonly Type Type;
        private readonly Func<object, bool> setResult;
        private readonly Func<Exception, bool> setException;
        private readonly SynchronizationContext syncContext;

        public DevToolsMethodResponseContext(Type type, Func<object, bool> setResult, Func<Exception, bool> setException, SynchronizationContext syncContext)
        {
            Type = type;
            this.setResult = setResult;
            this.setException = setException;
            this.syncContext = syncContext;
        }

        public void SetResult(object result)
        {
            InvokeOnSyncContext(setResult, result);
        }

        public void SetException(Exception ex)
        {
            InvokeOnSyncContext(setException, ex);
        }

        private void InvokeOnSyncContext<T>(Func<T, bool> fn, T value)
        {
            if (syncContext == null || syncContext == SynchronizationContext.Current)
            {
                fn(value);
            }
            else
            {
                // Using a KeyValuePair to pass the method and value into the callback to avoid capturing local variables in the delegate.
                syncContext.Post(new SendOrPostCallback(state =>
                {
                    var kv = (KeyValuePair<Func<T, bool>, T>)state;
                    kv.Key(kv.Value);
                }), new KeyValuePair<Func<T, bool>, T>(fn, value));
            }
        }
    }
}
