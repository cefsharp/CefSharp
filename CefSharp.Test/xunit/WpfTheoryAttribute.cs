// Code copied from https://github.com/xunit/samples.xunit/blob/5334ee9cf4a81f40dcb4cafabfeb098a555efb3d/STAExamples/WpfTheoryAttribute.cs

using System;
using Xunit;
using Xunit.Sdk;

namespace CefSharp.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("CefSharp.Test.WpfTheoryDiscoverer", "CefSharp.Test")]
    public class WpfTheoryAttribute : TheoryAttribute { }
}
