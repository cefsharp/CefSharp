using System;
using System.Threading;
using NUnit.Framework;

namespace CefSharp.Test
{
    [TestFixture]
    public class BrowserTest
    {
        [Test]
        public void FooTest()
        {
            Assert.Pass();
        }

        [Test]
        public void RunScriptText()
        {
            Assert.AreEqual("4", Fixture.Browser.EvaluateScript("2+2"));
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
