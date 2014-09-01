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

        public object CallMethod(long objectId, string name, object[] parameters)
        {
            object result;
            javascriptObjectRepository.TryCallMethod(objectId, name, parameters, out result);
            return result;
        }

        public object GetProperty(long objectId, string name)
        {
            object result;
            javascriptObjectRepository.TryGetProperty(objectId, name, out result);
            return result;
        }

        public void SetProperty(long objectId, string name, object value)
        {
            javascriptObjectRepository.TrySetProperty(objectId, name, value);
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
