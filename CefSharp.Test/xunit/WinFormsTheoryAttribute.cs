using System;
using Xunit;
using Xunit.Sdk;

namespace CefSharp.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("CefSharp.Test.WinFormsTheoryDiscoverer", "CefSharp.Test")]
    public class WinFormsTheoryAttribute : TheoryAttribute { }
}
