using System.ServiceModel;

namespace CefSharp.Internals
{
    public class BrowserProcessService : IBrowserProcess
    {
        private readonly BrowserProcessServiceHost host;
        
        public BrowserProcessService()
        {
            host = (BrowserProcessServiceHost)OperationContext.Current.Host;
            host.RenderProcess = OperationContext.Current.GetCallbackChannel<IRenderProcess>();
        }

        public object CallMethod(long objectId, string name, object[] parameters)
        {
            var javascriptObjectRepository = host.JavascriptObjectRepository;

            object result;
            javascriptObjectRepository.TryCallMethod(objectId, name, parameters, out result);
            return result;
        }

        public object GetProperty(long objectId, string name)
        {
            var javascriptObjectRepository = host.JavascriptObjectRepository;

            object result;
            javascriptObjectRepository.TryGetProperty(objectId, name, out result);
            return result;
        }

        public void SetProperty(long objectId, string name, object value)
        {
            var javascriptObjectRepository = host.JavascriptObjectRepository;
            javascriptObjectRepository.TrySetProperty(objectId, name, value);
        }

        public JavascriptObject GetRegisteredJavascriptObjects()
        {
            var javascriptObjectRepository = host.JavascriptObjectRepository;

            return javascriptObjectRepository.RootObject;
        }
    }
}
