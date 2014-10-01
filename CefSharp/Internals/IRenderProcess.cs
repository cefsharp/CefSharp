// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [ServiceContract]
    public interface IRenderProcess 
    {
        [OperationContract(AsyncPattern=true)]
        IAsyncResult BeginEvaluateScriptAsync(int browserId, long frameId, string script, TimeSpan? timeout, AsyncCallback callback, object state);

        JavascriptResponse EndEvaluateScriptAsync(IAsyncResult result);
    }
}
