// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Javascript
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavascriptCallbackTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;

        public JavascriptCallbackTests(ITestOutputHelper output, CefSharpFixture collectionFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
        }

        [Theory]
        [InlineData("(function() { return Promise.resolve(53)})", 53)]
        [InlineData("(function() { return Promise.resolve('53')})", "53")]
        [InlineData("(function() { return Promise.resolve(true)})", true)]
        [InlineData("(function() { return Promise.resolve(false)})", false)]
        public async Task ShouldWorkForPromise(string script, object expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            Assert.True(callbackResponse.Success);

            Assert.Equal(expected, callbackResponse.Result);

            output.WriteLine("Script {0} : Result {1}", script, callbackResponse.Result);
        }

        [Theory]
        [InlineData("(function() { return Promise.reject(new Error('My Error'))})", "Error: My Error")]
        [InlineData("(function() { return Promise.reject(42)})", "42")]
        [InlineData("(function() { return Promise.reject(false)})", "false")]
        [InlineData("(function() { return Promise.reject(null)})", "null")]
        public async Task ShouldFailForPromise(string script, string expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            Assert.False(callbackResponse.Success);

            Assert.Equal(expected, callbackResponse.Message);

            output.WriteLine("Script {0} : Message {1}", script, callbackResponse.Message);
        }

        [Theory]
        [InlineData(double.MaxValue, "Number.MAX_VALUE")]
        [InlineData(double.MaxValue / 2, "Number.MAX_VALUE / 2")]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForDoubleComputation(double expectedValue, string script)
        {
            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return " + script + "})");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            Assert.True(callbackResponse.Success);

            Assert.Equal(expectedValue, (double)callbackResponse.Result, 5);

            output.WriteLine("Script {0} : Result {1}", script, callbackResponse.Result);
        }

        [Theory]
        [InlineData(0.5d)]
        [InlineData(1.5d)]
        [InlineData(-0.5d)]
        [InlineData(-1.5d)]
        [InlineData(100000.24500d)]
        [InlineData(-100000.24500d)]
        [InlineData((double)uint.MaxValue)]
        [InlineData((double)int.MaxValue + 1)]
        [InlineData((double)int.MaxValue + 10)]
        [InlineData((double)int.MinValue - 1)]
        [InlineData((double)int.MinValue - 10)]
        [InlineData(((double)uint.MaxValue * 2))]
        [InlineData(((double)uint.MaxValue * 2) + 0.1)]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForDouble(double num)
        {
            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return " + num + "})");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            Assert.Equal(num, (double)callbackResponse.Result, 5);

            output.WriteLine("Expected {0} : Actual {1}", num, callbackResponse.Result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForInt(object num)
        {
            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return " + num.ToString() + "})");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            Assert.Equal(num, callbackResponse.Result);

            output.WriteLine("Expected {0} : Actual {1}", num, callbackResponse.Result);
        }

        [Theory]
        [InlineData("1970-01-01", "1970-01-01")]
        [InlineData("1980-01-01", "1980-01-01")]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldWorkForDate(DateTime expected, string actual)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return new Date('" + actual + "');})");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync();

            var actualDateTime = (DateTime)callbackResponse.Result;

            Assert.Equal(expected.ToLocalTime(), actualDateTime);

            output.WriteLine("Expected {0} : Actual {1}", expected.ToLocalTime(), actualDateTime);
        }

        [Theory]
        [InlineData("1970-01-01", DateTimeStyles.AssumeLocal)]
        [InlineData("1970-01-01", DateTimeStyles.AssumeUniversal)]
        [InlineData("1980-01-01", DateTimeStyles.AssumeLocal)]
        [InlineData("1980-01-01", DateTimeStyles.AssumeUniversal)]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldEchoDateTime(string expected, DateTimeStyles dateTimeStyle)
        {
            AssertInitialLoadComplete();

            var expectedDateTime = DateTime.Parse(expected, CultureInfo.InvariantCulture, dateTimeStyle);

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function(p) { return p; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync(expectedDateTime);

            var actualDateTime = (DateTime)callbackResponse.Result;

            Assert.Equal(expectedDateTime, actualDateTime);

            output.WriteLine("Expected {0} : Actual {1}", expectedDateTime, actualDateTime);
        }

        [Fact]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldEchoDateTimeNow()
        {
            AssertInitialLoadComplete();

            var expectedDateTime = DateTime.Now;

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function(p) { return p; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync(expectedDateTime);

            var actualDateTime = (DateTime)callbackResponse.Result;

            Assert.Equal(expectedDateTime, actualDateTime, TimeSpan.FromMilliseconds(10));

            output.WriteLine("Expected {0} : Actual {1}", expectedDateTime, actualDateTime);
        }
    }
}
