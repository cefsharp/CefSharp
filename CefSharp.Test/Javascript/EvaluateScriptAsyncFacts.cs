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
    public class EvaluateScriptAsyncFacts : IClassFixture<ChromiumWebBrowserOffScreenFixture>
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;
        private readonly ChromiumWebBrowserOffScreenFixture classFixture;

        public EvaluateScriptAsyncFacts(ITestOutputHelper output, CefSharpFixture collectionFixture, ChromiumWebBrowserOffScreenFixture classFixture)
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

            var javascriptResponse = await browser.EvaluateScriptAsync(script);
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();

            Assert.Equal(typeof(double), actualType);
            Assert.Equal(expectedValue, (double)javascriptResponse.Result, 5);

            output.WriteLine("Script {0} : Result {1}", script, javascriptResponse.Result);
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

            var javascriptResponse = await browser.EvaluateScriptAsync(num.ToString(CultureInfo.InvariantCulture));
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();

            Assert.Equal(typeof(double), actualType);
            Assert.Equal(num, (double)javascriptResponse.Result, 5);

            output.WriteLine("Expected {0} : Actual {1}", num, javascriptResponse.Result);
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

            var javascriptResponse = await browser.EvaluateScriptAsync(num.ToString());
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();

            Assert.Equal(typeof(int), actualType);
            Assert.Equal(num, javascriptResponse.Result);

            output.WriteLine("Expected {0} : Actual {1}", num, javascriptResponse.Result);
        }

        [Theory]
        [InlineData("1970-01-02", "1970-01-02")]
        [InlineData("1980-01-01", "1980-01-01")]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task CanEvaluateDateValues(DateTime expected, string actual)
        {
            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var javascriptResponse = await browser.EvaluateScriptAsync($"new Date('{actual}');");
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();
            var actualDateTime = (DateTime)javascriptResponse.Result;

            Assert.Equal(typeof(DateTime), actualType);
            Assert.Equal(expected.ToLocalTime(), actualDateTime);

            output.WriteLine("Expected {0} : Actual {1}", expected.ToLocalTime(), actualDateTime);
        }
    }
}
