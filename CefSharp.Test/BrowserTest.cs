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
            Assert.AreEqual("4", Fixture.Browser.RunScript("2+2"));
        }

        [Test]
        public void RunScriptConcurrentTest()
        {
            for (var x = 0; x < 3; x++)
            {
                var value = x.ToString();
                new Thread(() =>
                {
                    var result = Fixture.Browser.RunScript(value);
                    Console.WriteLine("{0} => {1}", value, result);
                    Assert.AreEqual(value, result);
                }).Start();
            }

            Thread.Sleep(1000);
        }
    }
}
