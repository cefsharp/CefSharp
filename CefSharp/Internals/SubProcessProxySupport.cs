using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
