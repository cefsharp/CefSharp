using CefSharp.Internals.JavascriptBinding;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CefSharp.BrowserSubprocess
{
    internal class JavascriptServiceHost
    {
        public static void Create(int browserId)
        {
            var uris = new[]
            {
                new Uri(JavascriptProxy.BaseAddress)
            };

            var host = new ServiceHost(typeof(JavascriptProxy), uris);
            AddDebugBehavior(host);

            // TODO: Include the name of the "parent process" here also, so you can run more than one CefSharp-based application
            // TODO: simultaneously. :)
            var serviceName = JavascriptProxy.ServiceName + "_" + browserId;

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