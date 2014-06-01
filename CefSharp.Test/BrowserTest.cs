using System;
using System.Threading;
using CefSharp.Wpf;
using Xunit;
using Xunit.Extensions;

namespace CefSharp.Test
{
    public class BrowserTest
    {
        [Theory()]
        [InlineData("'2'", "2")]
        [InlineData("2+2", 4)]
        public void EvaluateScriptTest(string script, object result)
        {
            using (var fixture = new Fixture())
            {
                fixture.Initialize().Wait();
                Assert.Equal(result, fixture.Browser.EvaluateScriptAsync(script));
            }
        }

        [Theory()]
        [InlineData("!!!")]
        public void EvaluateScriptExceptionTest(string script)
        {
            using (var fixture = new Fixture())
            {
                fixture.Initialize().Wait();
                Assert.Throws<ScriptException>(() =>
                    fixture.Browser.EvaluateScriptAsync(script));
            }
        }

        [Fact]
        public void RunScriptConcurrentTest()
        {
            //var threads = new List<Thread>(10);

            //for (var x = 0; x < 64; x++)
            //{
            //    var thread = new Thread(() =>
            //    {
            //        var script = x.ToString();
            //        Assert.AreEqual(x, Fixture.Browser.EvaluateScriptAsync(script));
            //    });
            //    threads.Add(thread);
            //    thread.Start();
            //}

            //threads.ForEach(x => x.Join());
        }
    }
}
