using System;
using Xunit;
using Xunit.Sdk;

namespace CefSharp.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("CefSharp.Test.WinFormsFactDiscoverer", "CefSharp.Test")]
    public class WinFormsFactAttribute : FactAttribute { }
}
