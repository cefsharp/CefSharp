// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CefSharp.Example;
using Xunit;
using Xunit.Abstractions;
using Xunit.Repeat;

namespace CefSharp.Test.Javascript
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class EvaluateScriptAsyncTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;

        public EvaluateScriptAsyncTests(ITestOutputHelper output, CefSharpFixture collectionFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
        }

        [Fact]
        public async Task V8Context()
        {
            Task evaluateCancelAfterDisposeTask;
            using (var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();

                // no V8 context
                await Assert.ThrowsAsync<Exception>(() => browser.EvaluateScriptAsync("1+1"));

                Task evaluateWithoutV8ContextCancelTask;
                Task<int> evaluateWithoutV8ContextTask;
                using (var frame = browser.GetMainFrame())
                {
                    evaluateWithoutV8ContextTask = frame.EvaluateScriptAsync<int>("1+2");
                    evaluateWithoutV8ContextCancelTask = frame.EvaluateScriptAsync("new Promise(resolve => setTimeout(resolve, 1000))");
                }

                // V8 context
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);
                var evaluateCancelAfterV8ContextChangeTask = browser.EvaluateScriptAsync("new Promise(resolve => setTimeout(resolve, 1000))");

                Assert.Equal(3, await evaluateWithoutV8ContextTask);
                Assert.Equal(4, await browser.EvaluateScriptAsync<int>("1+3"));

                // change V8 context
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);
                evaluateCancelAfterDisposeTask = browser.EvaluateScriptAsync("new Promise(resolve => setTimeout(resolve, 1000))");

                Assert.Equal(5, await browser.EvaluateScriptAsync<int>("1+4"));

                await Assert.ThrowsAsync<TaskCanceledException>(() => evaluateCancelAfterV8ContextChangeTask);
                await Assert.ThrowsAsync<TaskCanceledException>(() => evaluateWithoutV8ContextCancelTask);
            }
            await Assert.ThrowsAsync<TaskCanceledException>(() => evaluateCancelAfterDisposeTask);
        }

        [Fact]
        public async Task CancelEvaluateOnOOM()
        {
            await Assert.ThrowsAsync<TaskCanceledException>(() => Browser.EvaluateScriptAsync(
                @"
                let array1 = [];
                for (let i = 0; i < 10000000; i++) {
                    let array2 = [];
                    for (let j = 0; j < 10000000; j++) {
                        array2.push('a'.repeat(100000000));
                    }
                    array1.push(array2);
                }
                "
            ));
        }

        [Theory]
        [InlineData(double.MaxValue, "Number.MAX_VALUE")]
        [InlineData(double.MaxValue / 2, "Number.MAX_VALUE / 2")]
        //https://github.com/cefsharp/CefSharp/issues/3858
        public async Task ShouldWorkForDoubleComputation(double expectedValue, string script)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);
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
        public async Task ShouldWorkForDouble(double num)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(num.ToString(CultureInfo.InvariantCulture));
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
        public async Task ShouldWorkForInt(object num)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(num.ToString());
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();

            Assert.Equal(typeof(int), actualType);
            Assert.Equal(num, javascriptResponse.Result);

            output.WriteLine("Expected {0} : Actual {1}", num, javascriptResponse.Result);
        }

        [Theory]
        [InlineData("1970-01-01", "1970-01-01")]
        [InlineData("1980-01-01", "1980-01-01")]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldWorkForDate(DateTime expected, string actual)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync($"new Date('{actual}');");
            Assert.True(javascriptResponse.Success);

            var actualType = javascriptResponse.Result.GetType();
            var actualDateTime = (DateTime)javascriptResponse.Result;

            Assert.Equal(typeof(DateTime), actualType);
            Assert.Equal(expected.ToLocalTime(), actualDateTime);

            output.WriteLine("Expected {0} : Actual {1}", expected.ToLocalTime(), actualDateTime);
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { resolve(42); });", "42")]
        [InlineData("Promise.resolve(42);", "42")]
        [InlineData("(async () => { var result = await fetch('https://cefsharp.example/HelloWorld.html'); return result.status;})();", "200")]
        public async Task ShouldWorkForPromisePrimative(string script, string expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);

            Assert.True(javascriptResponse.Success);
            Assert.Equal(expected, javascriptResponse.Result.ToString());
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { reject('reject test'); });", "reject test")]
        [InlineData("(async () => { throw('reject test'); })();", "reject test")]
        public async Task ShouldFailForPromisePrimative(string script, string expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);

            Assert.False(javascriptResponse.Success);
            Assert.Equal(expected, javascriptResponse.Message);
        }

        [Theory]
        [InlineData("new Promise(function(resolve, reject) { resolve({ a: 'CefSharp', b: 42, }); });", "CefSharp", "42")]
        [InlineData("new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });", "CefSharp", "42")]
        [InlineData("(async () => { function sleep(ms) { return new Promise(resolve => setTimeout(resolve, ms)); }; async function getValAfterSleep() { await sleep(1000); return { a: 'CefSharp', b: 42 }; }; await sleep(2000); const result = await getValAfterSleep(); await sleep(2000); return result; })();", "CefSharp", "42")]
        public async Task ShouldWorkForPromisePrimativeObject(string script, string expectedA, string expectedB)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);

            Assert.True(javascriptResponse.Success);

            dynamic result = javascriptResponse.Result;
            Assert.Equal(expectedA, result.a.ToString());
            Assert.Equal(expectedB, result.b.ToString());
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
                var javascriptResponse = await Browser.EvaluateScriptAsync("2 + 2");

                if (javascriptResponse.Success)
                {
                    return (int)javascriptResponse.Result;
                }

                return -1;
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

            var result = await Browser.EvaluateScriptAsync(javascript);

            Assert.True(result.Success);
            Assert.Equal(expected, result.Result);
        }

        [Theory]
        [InlineData("return", "Uncaught SyntaxError: Illegal return statement\n@ about:blank:1:0")]
        public async Task CanEvaluateScriptAsyncReturnError(string javascript, string expected)
        {
            AssertInitialLoadComplete();

            var result = await Browser.EvaluateScriptAsync(javascript);

            Assert.False(result.Success);
            Assert.Equal(expected, result.Message);
        }

        /// <summary>
        /// Use the EvaluateScriptAsync (IWebBrowser, String,Object[]) overload and pass in string params
        /// that require encoding. Test case for https://github.com/cefsharp/CefSharp/issues/2339
        /// </summary>
        /// <returns>A task</returns>
        [Fact]
        public async Task CanEvaluateScriptAsyncWithEncodedStringArguments()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync("var testfunc=function(s) { return s; }");
            Assert.True(javascriptResponse.Success);

            // now call the function we just created
            string[] teststrings = new string[]{"Mary's\tLamb & \r\nOther Things",
                                      "[{test:\"Mary's Lamb & \\nOther Things\", 'other': \"\", 'and': null}]" };
            foreach (var test in teststrings)
            {
                javascriptResponse = await Browser.EvaluateScriptAsync("testfunc", test);
                Assert.True(javascriptResponse.Success);
                Assert.Equal(test, (string)javascriptResponse.Result);
                output.WriteLine("{0} passes {1}", test, javascriptResponse.Result);
            }
        }

        [Theory]
        [Repeat(20)]
        public async Task CanEvaluateScriptAsyncReturnArrayBuffer(int iteration)
        {
            AssertInitialLoadComplete();

            var randomizer = new Randomizer();

            var expected = randomizer.Utf16String(minLength: iteration, maxLength: iteration);
            var expectedBytes = Encoding.UTF8.GetBytes(expected);

            var javascriptResponse = await Browser.EvaluateScriptAsync($"new TextEncoder().encode('{expected}').buffer");

            Assert.True(javascriptResponse.Success);
            Assert.IsType<byte[]>(javascriptResponse.Result);

            var actualBytes = (byte[])javascriptResponse.Result;

            Assert.Equal(expectedBytes, actualBytes);

            Assert.Equal(expected, Encoding.UTF8.GetString(actualBytes));
        }

        [Theory]
        [InlineData("(async () => { function sleep(ms) { return new Promise(resolve => setTimeout(resolve, ms)); }; await sleep(2000); return true; })();")]
        public async Task ShouldTimeout(string script)
        {
            AssertInitialLoadComplete();

            var exception = await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await Browser.EvaluateScriptAsync(script, timeout: TimeSpan.FromMilliseconds(100)));

            Assert.NotNull(exception);
            Assert.IsType<TaskCanceledException>(exception);
        }
    }
}
