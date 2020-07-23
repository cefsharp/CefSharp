// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
#if !NETCOREAPP
using System.ServiceModel;
#endif

namespace CefSharp.Internals
{
#if !NETCOREAPP
    [ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceKnownType(typeof(object[]))]
    [ServiceKnownType(typeof(Dictionary<string, object>))]
    [ServiceKnownType(typeof(JavascriptObject))]
    [ServiceKnownType(typeof(JavascriptMethod))]
    [ServiceKnownType(typeof(JavascriptProperty))]
    [ServiceKnownType(typeof(JavascriptCallback))]
#endif
    public interface IBrowserProcess
    {
#if !NETCOREAPP
        [OperationContract]
#endif
        BrowserProcessResponse CallMethod(long objectId, string name, object[] parameters);

#if !NETCOREAPP
        [OperationContract]
#endif
        BrowserProcessResponse GetProperty(long objectId, string name);

#if !NETCOREAPP
        [OperationContract]
#endif
        BrowserProcessResponse SetProperty(long objectId, string name, object value);
    }
}
