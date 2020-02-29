// Code copied from https://github.com/xunit/samples.xunit/blob/5334ee9cf4a81f40dcb4cafabfeb098a555efb3d/STAExamples/WpfFactAttribute.cs

using System;
using Xunit;
using Xunit.Sdk;

namespace CefSharp.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("CefSharp.Test.WpfFactDiscoverer", "CefSharp.Test")]
    public class WpfFactAttribute : FactAttribute { }
}
