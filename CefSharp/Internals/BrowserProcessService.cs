// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.ServiceModel;

namespace CefSharp.Internals
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class BrowserProcessService : IBrowserProcess
    {
        private readonly JavascriptObjectRepository javascriptObjectRepository;
        private readonly BrowserProcessServiceHost host;
        public  OperationContext Context { get; private set; }
        
        public BrowserProcessService()
        {
            var context = OperationContext.Current;
            host = (BrowserProcessServiceHost)context.Host;

            javascriptObjectRepository = host.JavascriptObjectRepository;
        }
        
        public bool CallMethod(long objectId, string name, object[] parameters, out object result)
        {
            return javascriptObjectRepository.TryCallMethod(objectId, name, parameters, out result);
        }

        public bool GetProperty(long objectId, string name, out object result)
        {
            return javascriptObjectRepository.TryGetProperty(objectId, name, out result);
        }

        public bool SetProperty(long objectId, string name, object value)
        {
            return javascriptObjectRepository.TrySetProperty(objectId, name, value);
        }

        public JavascriptRootObject GetRegisteredJavascriptObjects()
        {
            if (Context == null)
            {
                Context = OperationContext.Current;
                host.SetOperationContext(Context);
            }
            
            return javascriptObjectRepository.RootObject;
        }
    }
}
