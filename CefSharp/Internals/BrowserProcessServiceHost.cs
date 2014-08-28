using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.Internals
{
    public class BrowserProcessServiceHost : ServiceHost
    {
        private const long SixteenMegaBytesInBytes = 16*1024*1024;

        public JavascriptObjectRepository JavascriptObjectRepository { get; private set; }
        public IRenderProcess RenderProcess { get; set; }
        
        public BrowserProcessServiceHost(JavascriptObjectRepository javascriptObjectRepository, int parentProcessId, int browserId)
            : base(typeof(BrowserProcessService), new Uri[0])
        {
            JavascriptObjectRepository = javascriptObjectRepository;

            var serviceName = RenderprocessClientFactory.GetServiceName(parentProcessId, browserId);

            Description.ApplyServiceBehavior(() => new ServiceDebugBehavior(), p => p.IncludeExceptionDetailInFaults = true);

            var binding = CreateBinding();

            var endPoint = AddServiceEndpoint(
                typeof(IBrowserProcess),
                binding,
                new Uri(serviceName)
            );

            endPoint.Contract.ProtectionLevel = ProtectionLevel.None;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            JavascriptObjectRepository = null;
            RenderProcess = null;
        }

        public static NetNamedPipeBinding CreateBinding()
        {
            var binding = new NetNamedPipeBinding();
            binding.MaxReceivedMessageSize = SixteenMegaBytesInBytes;
            return binding;
        }
    }
}