// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;
using Xunit.Extensions;

namespace CefSharp.Test
{
    public class BrowserTest
    {
        [Theory]
        [InlineData("'2'", "2")]
        [InlineData("2+2", 4)]
        public void EvaluateScriptTest(string script, object result)
        {
            using (var fixture = new Fixture())
            {
                fixture.Initialize().Wait();

                var task = fixture.Browser.EvaluateScriptAsync(script);

                task.Wait();

                Assert.Equal(result, task.Result);
            }
        }

        [Theory]
        [InlineData("!!!")]
        public void EvaluateScriptExceptionTest(string script)
        {
            using (var fixture = new Fixture())
            {
                fixture.Initialize().Wait();
                var task = fixture.Browser.EvaluateScriptAsync(script);
                task.Wait();
                Assert.Throws<ScriptException>(() => task.Result);
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
            //        Assert.AreEqual(x, Fixture.Browser.EvaluateScript(script));
            //    });
            //    threads.Add(thread);
            //    thread.Start();
            //}

            //threads.ForEach(x => x.Join());
        }
    }
}
