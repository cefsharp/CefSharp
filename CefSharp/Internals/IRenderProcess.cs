// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface IRenderProcess
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginJavascriptCallbackAsync(int browserId, long callbackId, object[] parameters, TimeSpan? timeout, AsyncCallback callback, object state);

        JavascriptResponse EndJavascriptCallbackAsync(IAsyncResult result);

        [OperationContract]
        void DestroyJavascriptCallback(int browserId, long id);
    }
}
