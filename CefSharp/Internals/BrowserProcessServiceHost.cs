using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.Internals
{
    public class BrowserProcessServiceHost : ServiceHost
    {
        public JavascriptObjectRepository JavascriptObjectRepository { get; private set; }
        public IRenderProcess RenderProcess { get; set; }
        
        public BrowserProcessServiceHost(JavascriptObjectRepository javascriptObjectRepository, int parentProcessId, int browserId)
            : base(typeof(BrowserProcessService), new Uri[0])
        {
            JavascriptObjectRepository = javascriptObjectRepository;

            var serviceName = RenderprocessClientFactory.GetServiceName(parentProcessId, browserId);

            Description.ApplyServiceBehavior(() => new ServiceDebugBehavior(), p => p.IncludeExceptionDetailInFaults = true);

            AddServiceEndpoint(
                typeof(IBrowserProcess),
                new NetNamedPipeBinding(),
                new Uri(serviceName)
            );
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            JavascriptObjectRepository = null;
            RenderProcess = null;
        }
    }
}