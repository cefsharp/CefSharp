using CefSharp.Internals;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.Internals
{
    public class BrowserprocessServiceHost : ServiceHost
    {
        public IBrowserProcess Browserprocess { get; private set; }
        public IRenderprocess Renderprocess { get; set; }
        
        public BrowserprocessServiceHost(IBrowserProcess browserprocess, int parentProcessId, int browserId)
            : base(typeof(BrowserProcessService), new Uri[0])
        {
            Browserprocess = browserprocess;

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
            Browserprocess = null;
            Renderprocess = null;
        }
    }
}