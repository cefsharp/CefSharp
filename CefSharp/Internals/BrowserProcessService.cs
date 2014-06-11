using System.ServiceModel;

namespace CefSharp.Internals
{
    public class BrowserProcessService : IBrowserProcess
    {
        private readonly BrowserProcessServiceHost host;
        
        public BrowserProcessService()
        {
            host = (BrowserProcessServiceHost)OperationContext.Current.Host;
            host.RenderProcess = OperationContext.Current.GetCallbackChannel<IRenderprocess>();
        }

        public object CallMethod(int objectId, string name, object[] parameters)
        {
            return host.BrowserProcess.CallMethod(objectId, name, parameters);
        }

        public object GetProperty(int objectId, string name)
        {
            return host.BrowserProcess.GetProperty(objectId, name);
        }

        public void SetProperty(int objectId, string name, object value)
        {
            host.BrowserProcess.SetProperty(objectId, name, value);
        }

        public JavascriptObject GetRegisteredJavascriptObjects()
        {
            return host.BrowserProcess.GetRegisteredJavascriptObjects();
        }
    }
}
