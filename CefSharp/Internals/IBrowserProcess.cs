// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(IRenderProcess), SessionMode = SessionMode.Required)]
    public interface IBrowserProcess
    {
        [OperationContract]
        bool CallMethod(long objectId, string name, object[] parameters, out object result);

        [OperationContract]
        bool GetProperty(long objectId, string name, out object result);

        [OperationContract]
        bool SetProperty(long objectId, string name, object value);
        
        [OperationContract]
        JavascriptRootObject GetRegisteredJavascriptObjects();
    }
}