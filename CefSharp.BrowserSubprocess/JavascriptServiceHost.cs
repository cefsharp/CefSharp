using CefSharp.Internals.JavascriptBinding;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.BrowserSubprocess
{
    internal class JavascriptServiceHost
    {
        public static void Create(int parentProcessId, int browserId)
        {
            var uris = new[]
            {
                new Uri(JavascriptProxySupport.BaseAddress)
            };

            var host = new ServiceHost(typeof(JavascriptProxy), uris);
            AddDebugBehavior(host);

            var serviceName = JavascriptProxySupport.GetServiceName(parentProcessId, browserId);
            Kernel32.OutputDebugString("Setting up IJavascriptProxy using service name: " + serviceName);
            host.AddServiceEndpoint(
                typeof(IJavascriptProxy),
                new NetNamedPipeBinding(),
                serviceName
            );

            host.Open();
        }

        private static void AddDebugBehavior(ServiceHostBase host)
        {
            var serviceDebugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            if (serviceDebugBehavior == null)
            {
                serviceDebugBehavior = new ServiceDebugBehavior
                {
                    IncludeExceptionDetailInFaults = true
                };
                host.Description.Behaviors.Add(serviceDebugBehavior);
            }
            else
            {
                serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            }
        }
    }
}