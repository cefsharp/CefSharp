// Code copied from https://github.com/xunit/samples.xunit/blob/5334ee9cf4a81f40dcb4cafabfeb098a555efb3d/STAExamples/WpfTheoryDiscoverer.cs

using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CefSharp.Test
{
    public class WpfTheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly TheoryDiscoverer theoryDiscoverer;

        public WpfTheoryDiscoverer(IMessageSink diagnosticMessageSink)
        {
            theoryDiscoverer = new TheoryDiscoverer(diagnosticMessageSink);
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return theoryDiscoverer.Discover(discoveryOptions, testMethod, factAttribute)
                                   .Select(testCase => new WpfTestCase(testCase));
        }
    }
}
