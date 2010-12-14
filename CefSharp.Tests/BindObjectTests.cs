namespace CefSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using NUnit.Framework;

    [TestFixture]
    public class BindObjectTests
    {
        private BrowserControl WebBrowser { get { return BrowserApplication.WebBrowser; } }

        #region Page Loading Tests
        [Test]
        public void Load1Test()
        {
            WebBrowser.Load("http://google.com/");
            WebBrowser.WaitForLoadCompletion();
        }

        /*
        [Test]
        public void Load2Test()
        {
            WebBrowser.Load("http://ya.ru/");
            WebBrowser.WaitForLoadCompletion();
        }

        [Test]
        public void Load3Test()
        {
            WebBrowser.Load("http://microsoft.com/");
            WebBrowser.WaitForLoadCompletion();
        }
        */
        #endregion

        #region Running Scripts

        private string RunScript(string script, int timeout)
        {
            var result = WebBrowser.RunScript("function(){" + script + "}()", "CefSharp.Tests", 1, timeout);
            return result;
        }

        private string RunScript(string script)
        {
            return RunScript(script, Timeout.Infinite);
        }

        [Test]
        public void InvalidScriptMustBeHandledTest()
        {
            try
            {
                var result = RunScript("some script with syntax error", 1000);
            }
            catch (TimeoutException)
            {
                Assert.Fail("Invalid script not executes - must generate exception. It useful when we have infinite or large timeout.");
                throw;
            }
            catch
            {
            }
        }

        [Test]
        public void Int_Echo_Int_Test()
        {
            var result = RunScript("return bound.EchoInt(67);");
            Assert.AreEqual("67", result);
        }

        [Test]
        public void String_Echo_String_Test()
        {
            var result1 = RunScript("return bound.EchoString('test string');");
            Assert.AreEqual("test string", result1);
        }

        [Test]
        public void StringUnicodeTest()
        {
            var sb = new StringBuilder();
            var sbc = new StringBuilder();
            sb.Append('\"');
            for (var c = 1; c <= 0xFFFF; c++)
            {
                if (c >= 0xD800 && c <= 0xDFFF) continue;

                sb.AppendFormat("\\u{0:X4}", c);
                sbc.Append((char)c);
            }
            sb.Append('\"');
            var testJsString = sb.ToString();
            var testString = sbc.ToString();
            
            var result2 = RunScript("return bound.EchoString("+testJsString+");");
            Assert.AreEqual(testString, result2);
        }

        [Test]
        public void StringSurrogatePairsTest()
        {
            // Supplementary Ideographic Plane
            // 20010
            var result = RunScript("return bound.EchoString('\uD840\uDC10');");

            if (result == "\xFFFD\xFFFD")
            {
                // TODO: http://www.russellcottrell.com/greek/utilities/SurrogatePairCalculator.htm
                // Try calculator to see that engine actually can display this chars.
                Assert.Fail("Who replace unicode surrogate pairs with REPLACEMENT CHARACTER?");
            }

            Assert.AreEqual("\xD840\xDC10", result);
        }
        #endregion

    }
}
