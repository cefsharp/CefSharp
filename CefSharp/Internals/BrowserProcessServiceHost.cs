using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.Internals
{
    public class BrowserProcessServiceHost : ServiceHost
    {
        public IBrowserProcess BrowserProcess { get; private set; }
        public IRenderProcess RenderProcess { get; set; }
        
        public BrowserProcessServiceHost(IBrowserProcess browserProcess, int parentProcessId, int browserId)
            : base(typeof(BrowserProcessService), new Uri[0])
        {
            BrowserProcess = browserProcess;

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
            BrowserProcess = null;
            RenderProcess = null;
        }
    }
}