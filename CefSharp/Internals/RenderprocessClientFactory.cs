using System;
using System.ServiceModel;

namespace CefSharp.Internals
{
    public class RenderprocessClientFactory
    {
        private const string BaseAddress = "net.pipe://localhost";
        private const string ServiceName = "CefSharpSubProcessProxy";

        public static string GetServiceName(int parentProcessId, int browserId)
        {
            return string.Join("/", BaseAddress, ServiceName, parentProcessId, browserId);
        }
    }
}
