// Code copied from https://github.com/xunit/samples.xunit/blob/master/STAExamples/WpfFactDiscoverer.cs

using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CefSharp.Test
{
    public class WpfFactDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly FactDiscoverer factDiscoverer;

        public WpfFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            factDiscoverer = new FactDiscoverer(diagnosticMessageSink);
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return factDiscoverer.Discover(discoveryOptions, testMethod, factAttribute)
                                 .Select(testCase => new WpfTestCase(testCase));
        }
    }
}
