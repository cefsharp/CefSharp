// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceContract(CallbackContract = typeof(IRenderProcess))]
    public interface IBrowserProcess
    {
        [OperationContract]
        object CallMethod(long objectId, string name, object[] parameters);

        [OperationContract]
        object GetProperty(long objectId, string name);

        [OperationContract]
        void SetProperty(long objectId, string name, object value);
        
        [OperationContract]
        JavascriptRootObject GetRegisteredJavascriptObjects();
    }
}