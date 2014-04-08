using System;
using System.ServiceModel;

namespace CefSharp.Internals
{
    public class SubprocessProxyFactory
    {
        private const string BaseAddress = "net.pipe://localhost";
        private const string ServiceName = "CefSharpSubProcessProxy";

        public static string GetServiceName(int parentProcessId, int browserId)
        {
            return string.Join("/", BaseAddress, ServiceName, parentProcessId, browserId);
        }

        public static ISubprocessProxy CreateSubprocessProxyClient(string serviceName,
            ISubprocessCallback callbackObject)
        {
            return CreateSubprocessProxyClient(serviceName, callbackObject, null);
        }

        public static ISubprocessProxy CreateSubprocessProxyClient(string serviceName, 
            ISubprocessCallback callbackObject, TimeSpan? timeout)
        {
            var channel = new DuplexChannelFactory<ISubprocessProxy>(
                callbackObject,
                new NetNamedPipeBinding(),
                new EndpointAddress(serviceName)
            );

            if (timeout != null)
            {
                channel.Open(timeout.Value);
            }
            else
            {
                channel.Open();
            }

            return channel.CreateChannel();
        }
    }
}
