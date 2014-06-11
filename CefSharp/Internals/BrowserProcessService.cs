using System.ServiceModel;

namespace CefSharp.Internals
{
    public class BrowserProcessService : IBrowserProcess
    {
        private readonly BrowserprocessServiceHost host;
        
        public BrowserProcessService()
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
