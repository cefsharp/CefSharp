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
    public class JavascriptCallbackFacts : IClassFixture<ChromiumWebBrowserOffScreenFixture>
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;
        private readonly ChromiumWebBrowserOffScreenFixture classFixture;

        public JavascriptCallbackFacts(ITestOutputHelper output, CefSharpFixture collectionFixture, ChromiumWebBrowserOffScreenFixture classFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
            this.classFixture = classFixture;
        }

        [Theory]
        [InlineData(double.MaxValue, "Number.MAX_VALUE")]
        [InlineData(double.MaxValue / 2, "Number.MAX_VALUE / 2")]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task CanEvaluateDoubleComputation(double expectedValue, string script)
        {
            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function() { return " + script + "})");
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
        public async Task CanEvaluateDoubleValues(double num)
        {
            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function() { return " + num + "})");
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
        public async Task CanEvaluateIntValues(object num)
        {
            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function() { return " + num.ToString() + "})");
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
        public async Task CanEvaluateDateValues(DateTime expected, string actual)
        {
            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function() { return new Date('" + actual + "');})");
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
        public async Task CanEchoDateTime(string expected, DateTimeStyles dateTimeStyle)
        {
            var expectedDateTime = DateTime.Parse(expected, CultureInfo.InvariantCulture, dateTimeStyle);

            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function(p) { return p; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync(expectedDateTime);

            var actualDateTime = (DateTime)callbackResponse.Result;

            Assert.Equal(expectedDateTime, actualDateTime);

            output.WriteLine("Expected {0} : Actual {1}", expectedDateTime, actualDateTime);
        }

        [Fact]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task CanEchoDateTimeNow()
        {
            var expectedDateTime = DateTime.Now;

            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync("(function(p) { return p; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync(expectedDateTime);

            var actualDateTime = (DateTime)callbackResponse.Result;

            Assert.Equal(expectedDateTime, actualDateTime, TimeSpan.FromMilliseconds(10));

            output.WriteLine("Expected {0} : Actual {1}", expectedDateTime, actualDateTime);
        }
    }
}
