using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CefSharp.Internals
{
    public class BrowserprocessService : IBrowserProcess
    {
        private BrowserprocessServiceHost host;
        
        public BrowserprocessService()
        {
            host = (BrowserprocessServiceHost)OperationContext.Current.Host;
            host.Renderprocess = OperationContext.Current.GetCallbackChannel<IRenderprocess>();
        }

        public object CallMethod(int objectId, string name, object[] parameters)
        {
            return host.Browserprocess.CallMethod(objectId, name, parameters);
        }

        public object GetProperty(int objectId, string name)
        {
            return host.Browserprocess.GetProperty(objectId, name);
        }

        public void SetProperty(int objectId, string name, object value)
        {
            host.Browserprocess.SetProperty(objectId, name, value);
        }

        public JavascriptObject GetRegisteredJavascriptObjects()
        {
            return host.Browserprocess.GetRegisteredJavascriptObjects();
        }
    }
}
