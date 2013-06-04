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
        private CefFormsWebBrowser WebBrowser { get { return BrowserApplication.WebBrowser; } }

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
            var result = WebBrowser.RunScript("function(){" + script + "}()");
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
| Boolean   | -         | -         | +         | -         | -        | -         | -         | -         |
*/
        [TestCase("Boolean_Undefined",     "bound.EchoBoolean(undefined), false",    true)]
        [TestCase("Boolean_Null",          "bound.EchoBoolean(null), false",         true)]
        [TestCase("Boolean_Boolean_True",  "bound.EchoBoolean(true) === true",       false)]
        [TestCase("Boolean_Boolean_False", "bound.EchoBoolean(false) === false",     false)]
        [TestCase("Boolean_Number_0",      "bound.EchoBoolean(0), false",            true)]
        [TestCase("Boolean_Number_1",      "bound.EchoBoolean(1), false",            true)]
        [TestCase("Boolean_String",        "bound.EchoBoolean('0'), false",          true)]
        [TestCase("Boolean_Date",          "bound.EchoBoolean(new Date()), false",   true)]
        [TestCase("Boolean_Array",         "bound.EchoBoolean([]), false",           true)]
        [TestCase("Boolean_Object",        "bound.EchoBoolean({}), false",           true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Boolean?  | *null     | *null     | +         |           |          |           |           |           |
*/
        [TestCase("Boolean?_Undefined",     "bound.EchoNullableBoolean(undefined) === null", false)]
        [TestCase("Boolean?_Null",          "bound.EchoNullableBoolean(null) === null",      false)]
        [TestCase("Boolean?_Boolean_True",  "bound.EchoNullableBoolean(true) === true",      false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| SByte     | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("SByte_Undefined",          "bound.EchoSByte(undefined), false",    true)]
        [TestCase("SByte_Null",               "bound.EchoSByte(null), false",         true)]
        [TestCase("SByte_Boolean",            "bound.EchoSByte(true), false",         true)]
        [TestCase("SByte_Number_-128",        "bound.EchoSByte(-128) === -128",       false)]
        [TestCase("SByte_Number_+127",        "bound.EchoSByte(+127) === +127",       false)]
        [TestCase("SByte_Number_+100.5",      "bound.EchoSByte(+100.5) === +100",     false)]
        [TestCase("SByte_Number_OverflowMin", "bound.EchoSByte(-129), false",         true)]
        [TestCase("SByte_Number_OverflowMax", "bound.EchoSByte(+128), false",         true)]
        [TestCase("SByte_String",             "bound.EchoSByte('0'), false",          true)]
        [TestCase("SByte_Date",               "bound.EchoSByte(new Date()), false",   true)]
        [TestCase("SByte_Array",              "bound.EchoSByte([]), false",           true)]
        [TestCase("SByte_Object",             "bound.EchoSByte({}), false",           true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| SByte?    | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("SByte?_Undefined",       "bound.EchoNullableSByte(undefined) === null",  false)]
        [TestCase("SByte?_Null",            "bound.EchoNullableSByte(null) === null",       false)]
        [TestCase("SByte?_Number_-128",     "bound.EchoNullableSByte(-128) === -128",       false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int16     | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Int16_Undefined",          "bound.EchoInt16(undefined), false",    true)]
        [TestCase("Int16_Null",               "bound.EchoInt16(null), false",         true)]
        [TestCase("Int16_Boolean",            "bound.EchoInt16(true), false",         true)]
        [TestCase("Int16_Number_-32768",      "bound.EchoInt16(-32768) === -32768",   false)]
        [TestCase("Int16_Number_+32767",      "bound.EchoInt16(+32767) === +32767",   false)]
        [TestCase("Int16_Number_+100.5",      "bound.EchoInt16(+100.5) === +100",     false)]
        [TestCase("Int16_Number_OverflowMin", "bound.EchoInt16(-32769), false",       true)]
        [TestCase("Int16_Number_OverflowMax", "bound.EchoInt16(+32768), false",       true)]
        [TestCase("Int16_String",             "bound.EchoInt16('0'), false",          true)]
        [TestCase("Int16_Date",               "bound.EchoInt16(new Date()), false",   true)]
        [TestCase("Int16_Array",              "bound.EchoInt16([]), false",           true)]
        [TestCase("Int16_Object",             "bound.EchoInt16({}), false",           true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int16?    | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Int16?_Undefined",       "bound.EchoNullableInt16(undefined) === null",  false)]
        [TestCase("Int16?_Null",            "bound.EchoNullableInt16(null) === null",       false)]
        [TestCase("Int16?_Number_-32768",   "bound.EchoNullableInt16(-32768) === -32768",   false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int32     | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Int32_Undefined",          "bound.EchoInt32(undefined), false",            true)]
        [TestCase("Int32_Null",               "bound.EchoInt32(null), false",                 true)]
        [TestCase("Int32_Boolean",            "bound.EchoInt32(true), false",                 true)]
        [TestCase("Int32_Number_-2147483648", "bound.EchoInt32(-2147483648) === -2147483648", false)]
        [TestCase("Int32_Number_+2147483647", "bound.EchoInt32(+2147483647) === +2147483647", false)]
        [TestCase("Int32_Number_+100.5",      "bound.EchoInt32(+100.5) === +100",             false)]
        [TestCase("Int32_Number_OverflowMin", "bound.EchoInt32(-2147483649), false",          true)]
        [TestCase("Int32_Number_OverflowMax", "bound.EchoInt32(+2147483648), false",          true)]
        [TestCase("Int32_String",             "bound.EchoInt32('0'), false",                  true)]
        [TestCase("Int32_Date",               "bound.EchoInt32(new Date()), false",           true)]
        [TestCase("Int32_Array",              "bound.EchoInt32([]), false",                   true)]
        [TestCase("Int32_Object",             "bound.EchoInt32({}), false",                   true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int32?    | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Int32?_Undefined",          "bound.EchoNullableInt32(undefined) === null",          false)]
        [TestCase("Int32?_Null",               "bound.EchoNullableInt32(null) === null",               false)]
        [TestCase("Int32?_Number_-2147483648", "bound.EchoNullableInt32(-2147483648) === -2147483648", false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int64     | -         | -         | -         | -         | -        | -         | -         | -         |
*/
        [TestCase("Int64_Undefined",          "bound.EchoInt64(undefined), false",              true)]
        [TestCase("Int64_Null",               "bound.EchoInt64(null), false",                   true)]
        [TestCase("Int64_Boolean",            "bound.EchoInt64(true), false",                   true)]
        [TestCase("Int64_Number_MinValue",    "bound.EchoInt64(-9223372036854775808), false",   true)]
        [TestCase("Int64_Number_MaxValue",    "bound.EchoInt64(+9223372036854775807), false",   true)]
        [TestCase("Int64_Number_+100.5",      "bound.EchoInt64(+100.5), false",                 true)]
        [TestCase("Int64_Number_OverflowMin", "bound.EchoInt64(-9223372036854775809), false",   true)]
        [TestCase("Int64_Number_OverflowMax", "bound.EchoInt64(+9223372036854775808), false",   true)]
        [TestCase("Int64_String_MinValue",    "bound.EchoInt64('-9223372036854775808'), false", true)]
        [TestCase("Int64_String_MaxValue",    "bound.EchoInt64('+9223372036854775807'), false", true)]
        [TestCase("Int64_Date",               "bound.EchoInt64(new Date()), false",             true)]
        [TestCase("Int64_Array",              "bound.EchoInt64([]), false",                     true)]
        [TestCase("Int64_Object",             "bound.EchoInt64({}), false",                     true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Int64?    | -         | -         | -         | -         | -        | -         | -         | -         |
*/
        [TestCase("Int64?_Undefined",       "bound.EchoNullableInt64(undefined) === null",          true)]
        [TestCase("Int64?_Null",            "bound.EchoNullableInt64(null) === null",               true)]
        [TestCase("Int64?_Number_MinValue", "bound.EchoNullableInt64(-9223372036854775808), false", true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Byte      | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Byte_Undefined",          "bound.EchoByte(undefined), false",  true)]
        [TestCase("Byte_Null",               "bound.EchoByte(null), false",       true)]
        [TestCase("Byte_Boolean",            "bound.EchoByte(true), false",       true)]
        [TestCase("Byte_Number_+255",        "bound.EchoByte(+255) === +255",     false)]
        [TestCase("Byte_Number_+100.5",      "bound.EchoByte(+100.5) === +100",   false)]
        [TestCase("Byte_Number_OverflowMin", "bound.EchoByte(-1), false",         true)]
        [TestCase("Byte_Number_OverflowMax", "bound.EchoByte(+256), false",       true)]
        [TestCase("Byte_String",             "bound.EchoByte('0'), false",        true)]
        [TestCase("Byte_Date",               "bound.EchoByte(new Date()), false", true)]
        [TestCase("Byte_Array",              "bound.EchoByte([]), false",         true)]
        [TestCase("Byte_Object",             "bound.EchoByte({}), false",         true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Byte?     | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Byte?_Undefined",   "bound.EchoNullableByte(undefined) === null", false)]
        [TestCase("Byte?_Null",        "bound.EchoNullableByte(null) === null",      false)]
        [TestCase("Byte?_Number_+255", "bound.EchoNullableByte(+255) === +255",      false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt16    | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("UInt16_Undefined",          "bound.EchoUInt16(undefined), false",  true)]
        [TestCase("UInt16_Null",               "bound.EchoUInt16(null), false",       true)]
        [TestCase("UInt16_Boolean",            "bound.EchoUInt16(true), false",       true)]
        [TestCase("UInt16_Number_+65535",      "bound.EchoUInt16(+65535) === +65535", false)]
        [TestCase("UInt16_Number_+100.5",      "bound.EchoUInt16(+100.5) === +100",   false)]
        [TestCase("UInt16_Number_OverflowMin", "bound.EchoUInt16(-1), false",         true)]
        [TestCase("UInt16_Number_OverflowMax", "bound.EchoUInt16(+65536), false",     true)]
        [TestCase("UInt16_String",             "bound.EchoUInt16('0'), false",        true)]
        [TestCase("UInt16_Date",               "bound.EchoUInt16(new Date()), false", true)]
        [TestCase("UInt16_Array",              "bound.EchoUInt16([]), false",         true)]
        [TestCase("UInt16_Object",             "bound.EchoUInt16({}), false",         true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt16?   | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("UInt16?_Undefined",     "bound.EchoNullableUInt16(undefined) === null", false)]
        [TestCase("UInt16?_Null",          "bound.EchoNullableUInt16(null) === null",      false)]
        [TestCase("UInt16?_Number_+65535", "bound.EchoNullableUInt16(+65535) === +65535",  false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt32    | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("UInt32_Undefined",          "bound.EchoUInt32(undefined), false",            true)]
        [TestCase("UInt32_Null",               "bound.EchoUInt32(null), false",                 true)]
        [TestCase("UInt32_Boolean",            "bound.EchoUInt32(true), false",                 true)]
        [TestCase("UInt32_Number_+4294967295", "bound.EchoUInt32(+4294967295) === +4294967295", false)]
        [TestCase("UInt32_Number_+100.5",      "bound.EchoUInt32(+100.5) === +100",             false)]
        [TestCase("UInt32_Number_OverflowMin", "bound.EchoUInt32(-1), false",                   true)]
        [TestCase("UInt32_Number_OverflowMax", "bound.EchoUInt32(+4294967296), false",          true)]
        [TestCase("UInt32_String",             "bound.EchoUInt32('0'), false",                  true)]
        [TestCase("UInt32_Date",               "bound.EchoUInt32(new Date()), false",           true)]
        [TestCase("UInt32_Array",              "bound.EchoUInt32([]), false",                   true)]
        [TestCase("UInt32_Object",             "bound.EchoUInt32({}), false",                   true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt32?   | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("UInt32?_Undefined",          "bound.EchoNullableUInt32(undefined) === null",          false)]
        [TestCase("UInt32?_Null",               "bound.EchoNullableUInt32(null) === null",               false)]
        [TestCase("UInt32?_Number_+4294967295", "bound.EchoNullableUInt32(+4294967295) === +4294967295", false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt64    | -         | -         | -         | -         | -        | -         | -         | -         |
*/
        [TestCase("UInt64_Undefined",          "bound.EchoUInt64(undefined), false",              true)]
        [TestCase("UInt64_Null",               "bound.EchoUInt64(null), false",                   true)]
        [TestCase("UInt64_Boolean",            "bound.EchoUInt64(true), false",                   true)]
        [TestCase("UInt64_Number_MinValue",    "bound.EchoUInt64(+0), false",                     true)]
        [TestCase("UInt64_Number_MaxValue",    "bound.EchoUInt64(+18446744073709551615), false",  true)]
        [TestCase("UInt64_Number_+100.5",      "bound.EchoUInt64(+100.5), false",                 true)]
        [TestCase("UInt64_Number_OverflowMin", "bound.EchoUInt64(-1), false",                     true)]
        [TestCase("UInt64_Number_OverflowMax", "bound.EchoUInt64(+18446744073709551616), false",  true)]
        [TestCase("UInt64_String_MinValue",    "bound.EchoUInt64('0'), false",                    true)]
        [TestCase("UInt64_String_MaxValue",    "bound.EchoUInt64('18446744073709551615'), false", true)]
        [TestCase("UInt64_Date",               "bound.EchoUInt64(new Date()), false",             true)]
        [TestCase("UInt64_Array",              "bound.EchoUInt64([]), false",                     true)]
        [TestCase("UInt64_Object",             "bound.EchoUInt64({}), false",                     true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| UInt64?   | -         | -         | -         | -         | -        | -         | -         | -         |
*/
        [TestCase("UInt64?_Undefined",       "bound.EchoNullableUInt64(undefined), false",              true)]
        [TestCase("UInt64?_Null",            "bound.EchoNullableUInt64(null), false",                   true)]
        [TestCase("UInt64?_Number_MaxValue", "bound.EchoNullableUInt64(18446744073709551615), false",   true)]
        [TestCase("UInt64?_String_MaxValue", "bound.EchoNullableUInt64('18446744073709551615'), false", true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Single    | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Single_Undefined",          "bound.EchoSingle(undefined), false",                         true)]
        [TestCase("Single_Null",               "bound.EchoSingle(null), false",                              true)]
        [TestCase("Single_Boolean",            "bound.EchoSingle(true), false",                              true)]
        [TestCase("Single_Number_MinValue",    "bound.EchoSingle(-3.402823e38) === -3.4028230607370965e+38", false)]
        [TestCase("Single_Number_MaxValue",    "bound.EchoSingle(+3.402823e38) === +3.4028230607370965e+38", false)]
//      [TestCase("Single_Number_+100.1234",   "bound.EchoSingle(+100.1234) === +100.1234",                  false)]
        [TestCase("Single_Number_OverflowMin", "bound.EchoSingle(-3.402824e38), false",                      true)]
        [TestCase("Single_Number_OverflowMax", "bound.EchoSingle(+3.402824e38), false",                      true)]
        [TestCase("Single_String_MaxValue",    "bound.EchoSingle(''), false",                                true)]
        [TestCase("Single_Date",               "bound.EchoSingle(new Date()), false",                        true)]
        [TestCase("Single_Array",              "bound.EchoSingle([]), false",                                true)]
        [TestCase("Single_Object",             "bound.EchoSingle({}), false",                                true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Single?   | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Single?_Undefined",       "bound.EchoNullableSingle(undefined) === null",                       false)]
        [TestCase("Single?_Null",            "bound.EchoNullableSingle(null) === null",                            false)]
        [TestCase("Single?_Number_MaxValue", "bound.EchoNullableSingle(+3.402823e38) === +3.4028230607370965e+38", false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Double    | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Double_Undefined",                    "bound.EchoDouble(undefined), false",                           true)]
        [TestCase("Double_Null",                         "bound.EchoDouble(null), false",                                true)]
        [TestCase("Double_Boolean",                      "bound.EchoDouble(true), false",                                true)]
        [TestCase("Double_Number_MinValue",              "bound.EchoDouble(Number.MIN_VALUE) === Number.MIN_VALUE",      false)]
        [TestCase("Double_Number_MaxValue",              "bound.EchoDouble(Number.MAX_VALUE) === Number.MAX_VALUE",      false)]
        [TestCase("Double_Number_+0.1234567890123456",   "bound.EchoDouble(+0.1234567890123456) === 0.1234567890123456", false)]
//      [TestCase("Double_Number_OverflowMin",           "bound.EchoDouble(-), false",                                   true)]
//      [TestCase("Double_Number_OverflowMax",           "bound.EchoDouble(-), false",                                   true)]
        [TestCase("Double_String_MaxValue",              "bound.EchoDouble(Number.MAX_VALUE.toString()), false",         true)]
        [TestCase("Double_Date",                         "bound.EchoDouble(new Date()), false",                          true)]
        [TestCase("Double_Array",                        "bound.EchoDouble([]), false",                                  true)]
        [TestCase("Double_Object",                       "bound.EchoDouble({}), false",                                  true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Double?   | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Double?_Undefined",       "bound.EchoNullableDouble(undefined) === null",                    false)]
        [TestCase("Double?_Null",            "bound.EchoNullableDouble(null) === null",                         false)]
        [TestCase("Double?_Number_MaxValue", "bound.EchoNullableDouble(Number.MAX_VALUE) === Number.MAX_VALUE", false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Char      | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Char_Undefined",          "bound.EchoChar(undefined), false",   true)]
        [TestCase("Char_Null",               "bound.EchoChar(null), false",        true)]
        [TestCase("Char_Boolean",            "bound.EchoChar(true), false",        true)]
        [TestCase("Char_Number_MinValue",    "bound.EchoChar(+0) === +0",          false)]
        [TestCase("Char_Number_MaxValue",    "bound.EchoChar(+65535) === +65535",  false)]
        [TestCase("Char_Number_OverflowMin", "bound.EchoChar(-1), false",          true)]
        [TestCase("Char_Number_OverflowMax", "bound.EchoChar(+65536), false",      true)]
        [TestCase("Char_String_Empty",       "bound.EchoChar(''), false",          true)]
        [TestCase("Char_String_TwoChar",     "bound.EchoChar('ab'), false",        true)]
        [TestCase("Char_String_Char",        "bound.EchoChar('c') === 63",         true)]
        [TestCase("Char_Date",               "bound.EchoChar(new Date()), false",  true)]
        [TestCase("Char_Array",              "bound.EchoChar([]), false",          true)]
        [TestCase("Char_Object",             "bound.EchoChar({}), false",          true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Char?     | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Char?_Undefined",       "bound.EchoNullableChar(undefined) === null", false)]
        [TestCase("Char?_Null",            "bound.EchoNullableChar(null) === null",      false)]
        [TestCase("Char?_Number_MaxValue", "bound.EchoNullableChar(+65535) === +65535",  false)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Decimal   | -         | -         | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Decimal_Undefined",          "bound.EchoDecimal(undefined), false",              true)]
        [TestCase("Decimal_Null",               "bound.EchoDecimal(null), false",                   true)]
        [TestCase("Decimal_Boolean",            "bound.EchoDecimal(true), false",                   true)]
//      [TestCase("Decimal_Number_MinValue",    "bound.EchoDecimal(Number.MIN_VALUE), false",       true)]
//      [TestCase("Decimal_Number_MaxValue",    "bound.EchoDecimal(Number.MAX_VALUE), false",       true)]
        [TestCase("Decimal_Number_+100.12345",  "bound.EchoDecimal(+100.12345) === +100.12345",     false)]
//      [TestCase("Decimal_Number_OverflowMin", "bound.EchoDecimal(-1), false",                     true)]
//      [TestCase("Decimal_Number_OverflowMax", "bound.EchoDecimal(+4294967296), false",            true)]
        [TestCase("Decimal_String_MaxValue",    "bound.EchoDecimal('18446744073709551615'), false", true)]
        [TestCase("Decimal_Date",               "bound.EchoDecimal(new Date()), false",             true)]
        [TestCase("Decimal_Array",              "bound.EchoDecimal([]), false",                     true)]
        [TestCase("Decimal_Object",             "bound.EchoDecimal({}), false",                     true)]
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| Decimal?  | *null     | *null     | -         | +         | -        | -         | -         | -         |
*/
        [TestCase("Decimal?_Undefined",       "bound.EchoNullableDecimal(undefined) === null",      false)]
        [TestCase("Decimal?_Null",            "bound.EchoNullableDecimal(null) === null",           false)]
//      [TestCase("Decimal?_Number_MaxValue", "bound.EchoNullableDecimal(Number.MAX_VALUE), false",       true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| String    | *null     | *null     | -         | -         | +        | -         | -         | -         |
*/
        [TestCase("String_Undefined",     "bound.EchoString(undefined) === null",  false)]
        [TestCase("String_Null",          "bound.EchoString(null) === null",       false)]
        [TestCase("String_Boolean",       "bound.EchoString(true), false",         true)]
        [TestCase("String_Number_1000",   "bound.EchoString(1000), false",         true)]
        [TestCase("String_Number_1000.5", "bound.EchoString(1000.5), false",       true)]
        [TestCase("String_String_Empty",  "bound.EchoString('') === ''",           false)]
        [TestCase("String_String_Value",  "bound.EchoString('Value') === 'Value'", false)]
        [TestCase("String_Date",          "bound.EchoString(new Date()), false",   true)]
        [TestCase("String_Array",         "bound.EchoString([]), false",           true)]
        [TestCase("String_Object",        "bound.EchoString({}), false",           true)]

/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| DateTime  | -         | -         | -         | -         | -        | +         | -         | -         |
*/
/*
|=CLR Type  |=JavaScript values and objects                                                                |
|           |=undefined |=null      |=Boolean   |=Number    |=String   |=Date      |=Array     |=Object    |
| DateTime? | -         | -         | -         | -         | -        | +         | -         | -         |
*/


        public void ExpectScriptExpr(string name, string jsExpr, bool expectScriptException)
        {
            string script = "return ((" + jsExpr + ")? 'pass' : 'fail');";
            string result = null;
            try
            {
                result = RunScript(script, 1000);
                if (expectScriptException)
                {
                    Assert.Fail("TestCase '{0}' with script '{1}' must be throw exception.", name, jsExpr);
                }
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
