using System.ServiceModel;

namespace CefSharp.Internals
{
    public class SubProcessProxySupport
    {
        private const string BaseAddress = "net.pipe://localhost";
        private const string ServiceName = "CefSharpSubProcessProxy";

        public static string GetServiceName(int parentProcessId, int browserId)
        {
            return string.Join("/", BaseAddress, ServiceName, parentProcessId, browserId);
        }

        public static DuplexChannelFactory<ISubProcessProxy> CreateChannelFactory(string serviceName, object callbackObject)
        {
            return new DuplexChannelFactory<ISubProcessProxy>(
                callbackObject,
                new NetNamedPipeBinding(),
                new EndpointAddress(serviceName)
            );
        }
    }
}
