using System;
using System.Reflection;
using CefSharp.Internals;
using Xunit.Sdk;

namespace CefSharp.Test
{
    internal class BrowserRefCountDebuggingAttribute : BeforeAfterTestAttribute
    {
        private Type type;
        internal BrowserRefCountDebuggingAttribute(Type type)
        {
            this.type = type;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            ((BrowserRefCounter)BrowserRefCounter.Instance).AppendLineToLog($"{type} - TestMethod {methodUnderTest.DeclaringType} {methodUnderTest.Name}");

            base.Before(methodUnderTest);
        }
    }
}
