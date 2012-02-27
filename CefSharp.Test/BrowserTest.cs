using System;
using System.Threading;
using NUnit.Framework;

namespace CefSharp.Test
{
    [TestFixture]
    public class BrowserTest
    {
        [TestCase("'2'", "2")]
        [TestCase("2+2", 4)]
        public void EvaluateScriptTest(string script, object result)
        {
            Assert.AreEqual(result, Fixture.Browser.EvaluateScript(script));
        }

        [TestCase("!!!")]
        public void EvaluateScriptExceptionTest(string script)
        {
            Assert.Throws<ScriptException>(() =>
                Fixture.Browser.EvaluateScript(script));
        }

        [Test]
        public void RunScriptConcurrentTest()
        {
            for (var x = 0; x < 10; x++)
            {
                var value = x.ToString();
                new Thread(() => Assert.AreEqual(value, Fixture.Browser.EvaluateScript(value))).
                    Start();
            }

            Thread.Sleep(1000);
        }

        [Test]
        public void RunScriptExceptionTest()
        {
            Assert.Throws<ScriptException>(() =>
                Fixture.Browser.EvaluateScript("!@#$%^"));
        }
    }
}
