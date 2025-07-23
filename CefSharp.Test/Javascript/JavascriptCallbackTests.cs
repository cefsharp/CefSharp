// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;
using CefSharp.Example;
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

        [Fact]
        public async Task V8Context()
        {
            IJavascriptCallback callbackExecuteCancelAfterDisposeCallback;
            Task callbackExecuteCancelAfterDisposeTask;
            using (var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();

                // no V8 context
                var withoutV8ContextException = await Assert.ThrowsAsync<Exception>(() => browser.EvaluateScriptAsync("(function() { return 1+1; })"));
                Assert.StartsWith("Unable to execute javascript at this time", withoutV8ContextException.Message);

                Task<JavascriptResponse> callbackExecuteWithoutV8ContextTask;
                using (var frame = browser.GetMainFrame())
                {
                    callbackExecuteWithoutV8ContextTask = frame.EvaluateScriptAsync("(function() { return 1+2; })");
                }

                // V8 context
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);

                var callbackExecuteWithoutV8ContextResponse = await callbackExecuteWithoutV8ContextTask;
                Assert.True(callbackExecuteWithoutV8ContextResponse.Success);
                var callbackExecuteWithoutV8ContextCallback = (IJavascriptCallback)callbackExecuteWithoutV8ContextResponse.Result;
                var callbackExecuteWithoutV8ContextExecuteResponse = await callbackExecuteWithoutV8ContextCallback.ExecuteAsync();
                Assert.True(callbackExecuteWithoutV8ContextExecuteResponse.Success);
                Assert.Equal(3, callbackExecuteWithoutV8ContextExecuteResponse.Result);

                var callbackExecuteCancelAfterV8ContextResponse = await browser.EvaluateScriptAsync("(function() { return new Promise(resolve => setTimeout(resolve, 1000)); })");
                Assert.True(callbackExecuteCancelAfterV8ContextResponse.Success);
                var callbackExecuteCancelAfterV8ContextCallback = (IJavascriptCallback)callbackExecuteCancelAfterV8ContextResponse.Result;
                var callbackExecuteCancelAfterV8ContextTask = callbackExecuteCancelAfterV8ContextCallback.ExecuteAsync();

                // change V8 context
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);

                await Assert.ThrowsAsync<TaskCanceledException>(() => callbackExecuteCancelAfterV8ContextTask);
                var callbackExecuteCancelAfterV8ContextResult = await callbackExecuteCancelAfterV8ContextCallback.ExecuteAsync();
                Assert.False(callbackExecuteCancelAfterV8ContextResult.Success);
                Assert.StartsWith("Unable to find JavascriptCallback with Id " + callbackExecuteCancelAfterV8ContextCallback.Id, callbackExecuteCancelAfterV8ContextResult.Message);

                var callbackExecuteCancelAfterDisposeResponse = await browser.EvaluateScriptAsync("(function() { return new Promise(resolve => setTimeout(resolve, 1000)); })");
                Assert.True(callbackExecuteCancelAfterDisposeResponse.Success);
                callbackExecuteCancelAfterDisposeCallback = (IJavascriptCallback)callbackExecuteCancelAfterDisposeResponse.Result;
                callbackExecuteCancelAfterDisposeTask = callbackExecuteCancelAfterDisposeCallback.ExecuteAsync();
            }
            Assert.False(callbackExecuteCancelAfterDisposeCallback.CanExecute);
            await Assert.ThrowsAsync<TaskCanceledException>(() => callbackExecuteCancelAfterDisposeTask);
            await Assert.ThrowsAsync<InvalidOperationException>(() => callbackExecuteCancelAfterDisposeCallback.ExecuteAsync());
        }

        [Fact]
        public async Task ShouldCancelOnCrash()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return new Promise(resolve => setTimeout(resolve, 1000)); })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var task = callback.ExecuteAsync();

            await Browser.LoadUrlAsync("chrome://crash");
            await Assert.ThrowsAsync<TaskCanceledException>(() => task);
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
            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return " + num.ToString(CultureInfo.InvariantCulture) + "})");
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

        [Fact]
        public async Task ShouldWorkWithExpandoObject()
        {
            AssertInitialLoadComplete();

            var expectedDateTime = DateTime.Now;

            dynamic request = new ExpandoObject();
            request.dateTime = expectedDateTime;

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function(p) { return p; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            var callbackResponse = await callback.ExecuteAsync(request);

            dynamic response = callbackResponse.Result;
            var actualDateTime = (DateTime)response.dateTime;

            Assert.Equal(expectedDateTime, actualDateTime, TimeSpan.FromMilliseconds(10));

            output.WriteLine("Expected {0} : Actual {1}", expectedDateTime, actualDateTime);
        }

        [Fact]
        public async Task ShouldWorkWhenExecutedMultipleTimes()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync("(function() { return 42; })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            for (var i = 0; i < 3; i++)
            {
                var callbackResponse = await callback.ExecuteAsync();
                Assert.True(callbackResponse.Success);
                Assert.Equal(42, callbackResponse.Result);
            }
        }
    }
}
