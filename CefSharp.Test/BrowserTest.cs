using System.Collections.Generic;
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
            var threads = new List<Thread>(10);

            for (var x = 0; x < 64; x++)
            {
                var thread = new Thread(() =>
                {
                    var script = x.ToString();
                    Assert.AreEqual(x, Fixture.Browser.EvaluateScript(script));
                });
                threads.Add(thread);
                thread.Start();
            }

            threads.ForEach(x => x.Join());
        }
    }
}
