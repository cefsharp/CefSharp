using CefSharp.Wpf;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;
using Xunit.Should;

namespace CefSharp.Test
{
    public class BrowserTest : ObjectBase
    {
        public BrowserTest()
        {
         
        }

        protected override void DoDispose(bool isDisposing)
        {
            base.DoDispose(isDisposing);
        }
        
        [Theory]
        [InlineData("'2'", "2")]
        [InlineData("2+2", 4)]
        public void EvaluateScriptTest(string script, object result)
        {
            using (var testwindow = new TestWindowWrapper())
            {
                testwindow.Window.WebView.EvaluateScript(script).Result.ShouldBe(result);
            }
        }
        [Theory]
        [InlineData("!!!")]
        public void EvaluateScriptExceptionTest(string script)
        {
            using (var testwindow = new TestWindowWrapper())
            {
                Assert.Throws<ScriptException>(() =>
                    testwindow.Window.WebView.EvaluateScript(script));
            }
        }

        [Fact]
        public void RunScriptConcurrentTest()
        {
            using (var testwindow = new TestWindowWrapper())
            {
                foreach (var item in Enumerable.Range( 0, 64 ).AsParallel())
                {
                    testwindow.Window.WebView.EvaluateScript(item.ToString()).Result.ToString();
                }
            }
        }
    }
}
