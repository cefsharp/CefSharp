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
        private CefWebBrowser WebBrowser { get { return BrowserApplication.WebBrowser; } }

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
        public void ScriptExceptionMustBeThrownTest()
        {
            Assert.Throws<ScriptException>(() =>
            {
                RunScript("return bound.__UnknownMethod();");
            });
        }

        [Test]
        public void ScriptExceptionMustBeThrownStringTest()
        {
            try
            {
                RunScript("throw 'my js exception message';");
            }
            catch (ScriptException ex)
            {
                Assert.AreEqual("my js exception message", ex.Message);
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void ScriptExceptionMustBeThrownObjectTest()
        {
            try
            {
                RunScript("throw {'type':'exception type', 'message':'my js exception message'};");
            }
            catch (ScriptException ex)
            {
                Assert.AreEqual("[object Object]", ex.Message);
                return;
            }
            Assert.Fail();
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

            var result2 = RunScript("return bound.EchoString(" + testJsString + ");");
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

        #region Object Members
        [Test]
        public void MethodIsMemberFunction()
        {
            var result = RunScript("return typeof(bound.EchoVoid);");
            Assert.AreEqual("function", result);
        }

        [Test]
        public void UndefinedMethodIsUndefined()
        {
            var result = RunScript("return bound.UndefinedMethod;");
            Assert.AreEqual("undefined", result);
        }
        #endregion

        #region JavaScript to CLR Type Conversion

        [Test]
        [TestCase("VoidRetValIsUndefined", "bound.EchoVoid() === undefined", false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Boolean   | *false    | *false    | +         | -         | -        | -         | -         | -         |
*/
        [TestCase("Boolean_Undefined",     "bound.EchoBoolean(undefined) === false", false)]
        [TestCase("Boolean_Null",          "bound.EchoBoolean(null) === false",      false)]
        [TestCase("Boolean_Boolean_True",  "bound.EchoBoolean(true) === true",       false)]
        [TestCase("Boolean_Boolean_False", "bound.EchoBoolean(false) === false",     false)]
        [TestCase("Boolean_Number_0",      "bound.EchoBoolean(0) === false",         true)]
        [TestCase("Boolean_Number_1",      "bound.EchoBoolean(1) === true",          true)]
        [TestCase("Boolean_String",        "bound.EchoBoolean('0') === true",        true)]
        [TestCase("Boolean_Date",          "bound.EchoBoolean(new Date()) === true", true)]
        [TestCase("Boolean_Array",         "bound.EchoBoolean([]) === true",         true)]
        [TestCase("Boolean_Object",        "bound.EchoBoolean({}) === true",         true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Boolean?  | *null     | *null     | +         | SAME      | SAME     | SAME      | SAME      | SAME      |
*/
        [TestCase("Boolean?_Undefined",     "bound.EchoNullableBoolean(undefined) === null", false)]
        [TestCase("Boolean?_Null",          "bound.EchoNullableBoolean(null) === null",      false)]
        [TestCase("Boolean?_Boolean_True",  "bound.EchoNullableBoolean(true) === true",      false)]
        [TestCase("Boolean?_Boolean_False", "bound.EchoNullableBoolean(false) === false",    false)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| SByte     | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int16     | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int32     | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int64     | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Byte      | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt16    | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt32    | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt64    | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Single    | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Double    | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Char      | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| DateTime  | -         | -         | -         | -         | -        | +         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Decimal   | -         | -         | -         | +         | -        | -         | -         | -         |
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| String    | -         | +         | -         | -         | +        | -         | -         | -         |
*/
        public void ExpectScriptExpr(string name, string jsExpr, bool expectScriptException)
        {
            string script = "return ((" + jsExpr + ")? 'pass' : 'fail');";
            string result = null;
            try
            {
                result = RunScript(script, 1000);
            }
            catch (TimeoutException te)
            {
                Assert.Fail("TestCase '{0}' with script '{1}' execution timed out. \r\n{2}", name, script, te.ToString());
            }
            catch (ScriptException se)
            {
                if (expectScriptException)
                {
                    Assert.Pass(se.Message);
                }
                else throw;
            }
            Assert.AreEqual("pass", result, "TestCase '{0}' with script '{1}' fails.", name, script);
        }

        #endregion
    }
}
