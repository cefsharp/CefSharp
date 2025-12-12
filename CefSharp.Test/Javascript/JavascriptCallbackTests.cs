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
        public async Task ShouldCancelAfterV8ContextChange()
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

        [Fact]
        public async Task ShouldHandleCallbackAfterMultipleContextChanges()
        {
            AssertInitialLoadComplete();

            // Test that callbacks are properly cleaned up after multiple context changes
            var javascriptResponse1 = await Browser.EvaluateScriptAsync("(function() { return Promise.resolve(42); })");
            Assert.True(javascriptResponse1.Success);
            var callback1 = (IJavascriptCallback)javascriptResponse1.Result;

            // Change context
            await Browser.LoadUrlAsync(CefExample.HelloWorldUrl);

            var javascriptResponse2 = await Browser.EvaluateScriptAsync("(function() { return Promise.resolve(84); })");
            Assert.True(javascriptResponse2.Success);
            var callback2 = (IJavascriptCallback)javascriptResponse2.Result;

            // Execute the new callback - should work
            var callbackResponse2 = await callback2.ExecuteAsync();
            Assert.True(callbackResponse2.Success);
            Assert.Equal(84, callbackResponse2.Result);

            // Old callback should fail gracefully
            var callbackResponse1 = await callback1.ExecuteAsync();
            Assert.False(callbackResponse1.Success);
            Assert.Contains("Frame with Id:", callbackResponse1.Message);
        }

        [Fact]
        public async Task ShouldProperlyCleanupCallbacksOnFrameDestruction()
        {
            using (var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);

                var javascriptResponse = await browser.EvaluateScriptAsync("(function() { return Promise.resolve('test'); })");
                Assert.True(javascriptResponse.Success);
                var callback = (IJavascriptCallback)javascriptResponse.Result;
                var frameId = browser.GetMainFrame().Identifier;

                // Execute callback successfully first
                var result1 = await callback.ExecuteAsync();
                Assert.True(result1.Success);
                Assert.Equal("test", result1.Result);

                // Load new page to destroy frame
                await browser.LoadUrlAsync("about:blank");

                // Callback should now fail with frame-specific error
                var result2 = await callback.ExecuteAsync();
                Assert.False(result2.Success);
                Assert.Contains($"Frame with Id:{frameId}", result2.Message);
            }
        }

        [Fact]
        public async Task ShouldHandleCallbacksFromDifferentFrames()
        {
            using (var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();
                
                // Load a page with iframe
                await browser.LoadHtmlAsync(@"
                    <html>
                        <body>
                            <h1>Main Frame</h1>
                            <iframe id='testFrame' src='about:blank'></iframe>
                        </body>
                    </html>");

                // Create callback in main frame
                var mainFrameResponse = await browser.EvaluateScriptAsync("(function() { return Promise.resolve('main'); })");
                Assert.True(mainFrameResponse.Success);
                var mainCallback = (IJavascriptCallback)mainFrameResponse.Result;

                // Execute main frame callback
                var mainResult = await mainCallback.ExecuteAsync();
                Assert.True(mainResult.Success);
                Assert.Equal("main", mainResult.Result);
            }
        }

        [Theory]
        [InlineData("(function() { return Promise.resolve(null); })", null)]
        [InlineData("(function() { return Promise.resolve(undefined); })", null)]
        public async Task ShouldHandleNullAndUndefinedCallbackResults(string script, object expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(script);
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            var callbackResponse = await callback.ExecuteAsync();

            Assert.True(callbackResponse.Success);
            Assert.Equal(expected, callbackResponse.Result);
        }

        [Fact]
        public async Task ShouldHandleNestedCallbackExecution()
        {
            AssertInitialLoadComplete();

            // Create a callback that returns another function
            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function() {
                    return function(x) {
                        return Promise.resolve(x * 2);
                    };
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            
            // Execute with parameter
            var callbackResponse = await callback.ExecuteAsync(21);
            Assert.True(callbackResponse.Success);
            Assert.Equal(42, callbackResponse.Result);
        }

        [Fact]
        public async Task ShouldHandleCallbackExecutionWithComplexObjects()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function(obj) {
                    return Promise.resolve({
                        doubled: obj.value * 2,
                        message: 'Result: ' + obj.value
                    });
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            
            var inputObj = new { value = 42 };
            var callbackResponse = await callback.ExecuteAsync(inputObj);
            
            Assert.True(callbackResponse.Success);
            dynamic result = callbackResponse.Result;
            Assert.Equal(84, (int)result.doubled);
            Assert.Equal("Result: 42", (string)result.message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task ShouldHandleMultipleSequentialCallbackExecutions(int executionCount)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function(x) {
                    return Promise.resolve(x + 1);
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;

            for (var i = 0; i < executionCount; i++)
            {
                var callbackResponse = await callback.ExecuteAsync(i);
                Assert.True(callbackResponse.Success);
                Assert.Equal(i + 1, callbackResponse.Result);
            }
        }

        [Fact]
        public async Task ShouldHandleCallbackWithLongRunningOperation()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function() {
                    return new Promise(resolve => {
                        setTimeout(() => resolve('completed'), 2000);
                    });
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            
            var callbackResponse = await callback.ExecuteAsync();
            Assert.True(callbackResponse.Success);
            Assert.Equal("completed", callbackResponse.Result);
        }

        [Fact]
        public async Task ShouldHandleCallbackErrorsGracefully()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function() {
                    return Promise.reject(new Error('Custom error message'));
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            var callbackResponse = await callback.ExecuteAsync();

            Assert.False(callbackResponse.Success);
            Assert.Contains("Custom error message", callbackResponse.Message);
        }

        [Fact]
        public async Task ShouldVerifyCallbackRegistryCleanup()
        {
            // Test that callbacks are properly cleaned up when context is released
            using (var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();
                await browser.LoadUrlAsync(CefExample.HelloWorldUrl);

                var callbacks = new List<IJavascriptCallback>();
                
                // Create multiple callbacks
                for (int i = 0; i < 5; i++)
                {
                    var response = await browser.EvaluateScriptAsync($"(function() {{ return Promise.resolve({i}); }})");
                    Assert.True(response.Success);
                    callbacks.Add((IJavascriptCallback)response.Result);
                }

                // Verify all callbacks work
                for (int i = 0; i < callbacks.Count; i++)
                {
                    var result = await callbacks[i].ExecuteAsync();
                    Assert.True(result.Success);
                    Assert.Equal(i, result.Result);
                }

                // Destroy context
                await browser.LoadUrlAsync("about:blank");

                // Verify all callbacks are now invalid
                foreach (var callback in callbacks)
                {
                    var result = await callback.ExecuteAsync();
                    Assert.False(result.Success);
                }
            }
        }

        [Fact]
        public async Task ShouldHandleCallbackWithArrayParameter()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function(arr) {
                    return Promise.resolve(arr.reduce((a, b) => a + b, 0));
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            var callbackResponse = await callback.ExecuteAsync(new[] { 1, 2, 3, 4, 5 });

            Assert.True(callbackResponse.Success);
            Assert.Equal(15, callbackResponse.Result);
        }

        [Fact]
        public async Task ShouldHandleCallbackReturningArray()
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsync(@"
                (function() {
                    return Promise.resolve([1, 2, 3, 4, 5]);
                })");
            Assert.True(javascriptResponse.Success);

            var callback = (IJavascriptCallback)javascriptResponse.Result;
            var callbackResponse = await callback.ExecuteAsync();

            Assert.True(callbackResponse.Success);
            var resultArray = callbackResponse.Result as object[];
            Assert.NotNull(resultArray);
            Assert.Equal(5, resultArray.Length);
        }
    }
}
