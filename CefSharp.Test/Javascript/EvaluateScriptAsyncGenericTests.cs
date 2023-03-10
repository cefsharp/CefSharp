// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Javascript
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class EvaluateScriptAsyncGenericTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;

        public EvaluateScriptAsyncGenericTests(ITestOutputHelper output, CefSharpFixture collectionFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
        }

        [Theory]
        [InlineData(double.MaxValue, "Number.MAX_VALUE")]
        [InlineData(double.MaxValue / 2, "Number.MAX_VALUE / 2")]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForDoubleComputation(double expectedValue, string script)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<double>(script);

            Assert.Equal(expectedValue, actual);
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
        public async Task ShouldWorkForDouble(double expected)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<double>(expected.ToString(CultureInfo.InvariantCulture));

            Assert.Equal(expected, actual, 5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForInt(object expected)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<int>(expected.ToString());

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1970-01-01", "1970-01-01")]
        [InlineData("1980-01-01", "1980-01-01")]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldWorkForDate(DateTime expected, string str)
        {
            AssertInitialLoadComplete();

            expected = expected.ToLocalTime();

            var actual = await Browser.EvaluateScriptAsync<DateTime>($"new Date('{str}');");

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { resolve(42); });", "42")]
        [InlineData("Promise.resolve(42);", "42")]
        [InlineData("(async () => { var result = await fetch('https://cefsharp.example/HelloWorld.html'); return result.status;})();", "200")]
        public async Task ShouldWorkForPromisePrimative(string script, string expected)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<string>(script);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { reject('reject test'); });", "reject test")]
        [InlineData("(async () => { throw('reject test'); })();", "reject test")]
        public async Task ShouldFailForPromisePrimative(string script, string expected)
        {
            AssertInitialLoadComplete();

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await Browser.EvaluateScriptAsync<string>(script);
            });

            Assert.Equal(expected, exception.Message);
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { resolve({ a: 'CefSharp', b: 42, }); });", "CefSharp", "42")]
        [InlineData("new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });", "CefSharp", "42")]
        [InlineData("(async () => { function sleep(ms) { return new Promise(resolve => setTimeout(resolve, ms)); }; async function getValAfterSleep() { await sleep(1000); return { a: 'CefSharp', b: 42 }; }; await sleep(2000); const result = await getValAfterSleep(); await sleep(2000); return result; })();", "CefSharp", "42")]
        public async Task ShouldWorkForPromisePrimativeObject(string script, string expectedA, string expectedB)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<dynamic>(script);

            Assert.Equal(expectedA, actual.a.ToString());
            Assert.Equal(expectedB, actual.b.ToString());
        }

        [Fact]
        public async Task ShouldLoadGoogleAndEvaluateScript()
        {
            AssertInitialLoadComplete();

            var loadResponse = await Browser.LoadUrlAsync("www.google.com");

            Assert.True(loadResponse.Success);

            var mainFrame = Browser.GetMainFrame();
            Assert.True(mainFrame.IsValid);
            Assert.Contains("www.google", mainFrame.Url);

            var response = await Browser.EvaluateScriptAsync<int>("2 + 2");
            Assert.Equal(4, response);
            output.WriteLine("Result of 2 + 2: {0}", response);
        }

        [Fact]
        public async Task CanEvaluateScriptInParallel()
        {
            AssertInitialLoadComplete();

            var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(async () =>
            {
                return await Browser.EvaluateScriptAsync<int>("2 + 2");
            })).ToList();

            await Task.WhenAll(tasks);

            Assert.All(tasks, (t) =>
            {
                Assert.Equal(4, t.Result);
            });
        }

        [Theory]
        [InlineData("[1,2,,5]", new object[] { 1, 2, null, 5 })]
        [InlineData("[1,2,,]", new object[] { 1, 2, null })]
        [InlineData("[,2,3]", new object[] { null, 2, 3 })]
        [InlineData("[,2,,3,,4,,,,5,,,]", new object[] { null, 2, null, 3, null, 4, null, null, null, 5, null, null })]
        public async Task CanEvaluateScriptAsyncReturnPartiallyEmptyArrays(string javascript, object[] expected)
        {
            AssertInitialLoadComplete();

            var actual = await Browser.EvaluateScriptAsync<object[]>(javascript);

            Assert.Equal(expected, actual);
        }
    }
}
