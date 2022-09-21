using System.Reflection;
using CefSharp.Internals;
using Xunit.Sdk;

namespace CefSharp.Test
{
    internal class BrowserRefCountDebuggingAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            ((BrowserRefCounter)BrowserRefCounter.Instance).AppendLineToLog($"Test Method {methodUnderTest.DeclaringType} {methodUnderTest.Name}");

            base.Before(methodUnderTest);
        }
    }
}
