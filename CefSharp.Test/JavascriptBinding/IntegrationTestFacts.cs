// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.Event;
using CefSharp.Example;
using CefSharp.Example.JavascriptBinding;
using CefSharp.Example.ModelBinding;
using CefSharp.Internals;
using CefSharp.ModelBinding;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.JavascriptBinding
{
    /// <summary>
    /// This is more a set of integration tests than it is unit tests, for now we need to
    /// run our QUnit tests in an automated fashion and some other testing.
    /// </summary>
    //TODO: Improve Test Naming, we need a naming scheme that fits these cases that's consistent
    //(Ideally we implement consistent naming accross all test classes, though I'm open to a different
    //naming convention as these are more integration tests than unit tests).
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class IntegrationTestFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public IntegrationTestFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

#if NETCOREAPP
        [Fact]
        public async Task LoadJavaScriptBindingQunitTestsSuccessfulCompletion()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingTestNetCoreUrl, automaticallyCreateBrowser: false))
            {
                //TODO: Extract this into some sort of helper setup method
                var bindingOptions = BindingOptions.DefaultBinder;
                var repo = browser.JavascriptObjectRepository;

                repo.Register("boundAsync", new AsyncBoundObject(), options: bindingOptions);
                repo.Register("boundAsync2", new AsyncBoundObject(), options: bindingOptions);

                browser.JavascriptMessageReceived += (s, e) =>
                {
                    dynamic msg = e.Message;
                    var type = (string)msg.Type;

                    if (type == "QUnitTestFailed")
                    {
                        var testOutput = (string)msg.Output;
                        output.WriteLine(testOutput);
                    }
                };

                var response = await browser.CreateBrowserAndWaitForQUnitTestExeuctionToComplete();

                if (!response.Success)
                {
                    output.WriteLine("QUnit Passed : {0}", response.Passed);
                    output.WriteLine("QUnit Total : {0}", response.Total);
                }

                Assert.True(response.Success);

                output.WriteLine("QUnit Tests result: {0}", response.Success);
            }
        }
#else
        [Fact(Skip = "Issue https://github.com/cefsharp/CefSharp/issues/3867")]
        public async Task LoadJavaScriptBindingQunitTestsSuccessfulCompletion()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingTestUrl, automaticallyCreateBrowser: false))
            {
                //TODO: Extract this into some sort of helper setup method
                var bindingOptions = BindingOptions.DefaultBinder;
                var repo = browser.JavascriptObjectRepository;

                repo.Register("bound", new BoundObject(), isAsync: false, options: bindingOptions);
                repo.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                repo.Register("boundAsync2", new AsyncBoundObject(), isAsync: true, options: bindingOptions);

                browser.JavascriptMessageReceived += (s, e) =>
                {
                    dynamic msg = e.Message;
                    var type = (string)msg.Type;

                    if (type == "QUnitTestFailed")
                    {
                        var testOutput = (string)msg.Output;
                        output.WriteLine(testOutput);
                    }
                };

                var response = await browser.CreateBrowserAndWaitForQUnitTestExeuctionToComplete();

                if (!response.Success)
                {
                    output.WriteLine("QUnit Passed : {0}", response.Passed);
                    output.WriteLine("QUnit Total : {0}", response.Total);
                }

                Assert.True(response.Success);

                output.WriteLine("QUnit Tests result: {0}", response.Success);
            }
        }

        [Fact]
        public async Task LoadJavaScriptBindingAsyncTaskQunitTestsSuccessfulCompletion()
        {
            CefSharpSettings.ConcurrentTaskExecution = true;

            using (var browser = new ChromiumWebBrowser(CefExample.BindingTestsAsyncTaskUrl, automaticallyCreateBrowser: false))
            {
                CefSharpSettings.ConcurrentTaskExecution = false;

                //TODO: Extract this into some sort of helper setup method
                var bindingOptions = BindingOptions.DefaultBinder;
                // intercept .net methods calls from js and log it
                bindingOptions.MethodInterceptor = new MethodInterceptorLogger();
                var repo = browser.JavascriptObjectRepository;

                repo.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);

                browser.JavascriptMessageReceived += (s, e) =>
                {
                    dynamic msg = e.Message;
                    var type = (string)msg.Type;

                    if (type == "QUnitTestFailed")
                    {
                        var testOutput = (string)msg.Output;
                        output.WriteLine(testOutput);
                    }
                };

                browser.LoadError += (s, e) =>
                {
                    var err = e.ErrorCode;
                };

                var response = await browser.CreateBrowserAndWaitForQUnitTestExeuctionToComplete();

                if (!response.Success)
                {
                    output.WriteLine("QUnit Passed : {0}", response.Passed);
                    output.WriteLine("QUnit Total : {0}", response.Total);
                }

                Assert.True(response.Success);

                output.WriteLine("QUnit Tests result: {0}", response.Success);
            }
        }

        [SkipIfRunOnAppVeyorFact()]
        //Skipping Issue https://github.com/cefsharp/CefSharp/issues/3867
        public async Task LoadLegacyJavaScriptBindingQunitTestsSuccessfulCompletion()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.LegacyBindingTestUrl, automaticallyCreateBrowser: false))
            {
                //TODO: Extract this into some sort of helper setup method
                var bindingOptions = BindingOptions.DefaultBinder;
                var repo = browser.JavascriptObjectRepository;
                repo.Settings.LegacyBindingEnabled = true;

                repo.Register("bound", new BoundObject(), isAsync: false, options: bindingOptions);
                repo.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);

                browser.JavascriptMessageReceived += (s, e) =>
                {
                    dynamic msg = e.Message;
                    var type = (string)msg.Type;

                    if (type == "QUnitTestFailed")
                    {
                        var testOutput = (string)msg.Output;
                        output.WriteLine(testOutput);
                    }
                };

                var response = await browser.CreateBrowserAndWaitForQUnitTestExeuctionToComplete();

                if(!response.Success)
                {
                    output.WriteLine("QUnit Passed : {0}", response.Passed);
                    output.WriteLine("QUnit Total : {0}", response.Total);
                }

                Assert.True(response.Success);

                output.WriteLine("QUnit Tests result: {0}", response.Success);
            }
        }
#endif

        [Fact]
        public async Task IsObjectCachedWithInvalidObjectNameReturnsFalse()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl))
            {
                await browser.WaitForInitialLoadAsync();

                //We'll execute twice using the different cased (camelcase naming and standard)
                var response = await browser.EvaluateScriptAsync("CefSharp.IsObjectCached('doesntexist')");

                Assert.True(response.Success);
                Assert.False((bool)response.Result);

                response = await browser.EvaluateScriptAsync("cefSharp.isObjectCached('doesntexist')");

                Assert.True(response.Success);
                Assert.False((bool)response.Result);
            }
        }

        [Fact]
        public async Task JsBindingGlobalObjectNameCustomValueExecuteIsObjectCachedSuccess()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiGlobalObjectName = "bindingApiObject";

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                await browser.WaitForInitialLoadAsync();

                var result = await browser.EvaluateScriptAsync("bindingApiObject.isObjectCached('doesntexist') === false");

                Assert.True(result.Success);
            }
        }

        [Fact]
        public async Task JsBindingGlobalApiDisabled()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = false;

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                var loadResponse = await browser.WaitForInitialLoadAsync();

                Assert.True(loadResponse.Success);

                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");

                Assert.True(response1.Success);
                Assert.True((bool)response1.Result);

                Assert.True(response2.Success);
                Assert.True((bool)response2.Result);
            }
        }

        [Fact]
        public async Task JsBindingGlobalApiEnabled()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                await browser.WaitForInitialLoadAsync();

                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");

                Assert.True(response1.Success);
                Assert.False((bool)response1.Result);

                Assert.True(response2.Success);
                Assert.False((bool)response2.Result);
            }
        }

        [Theory]
        [InlineData("CefSharp.RenderProcessId")]
        [InlineData("cefSharp.renderProcessId")]
        public async Task JsBindingRenderProcessId(string script)
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl))
            {
                await browser.WaitForInitialLoadAsync();

                var result = await browser.EvaluateScriptAsync(script);

                Assert.True(result.Success);

                using (var process = Process.GetProcessById(Assert.IsType<int>(result.Result)))
                {
                    Assert.Equal("CefSharp.BrowserSubprocess", process.ProcessName);
                }
            }
        }

        [Fact]
        //Issue https://github.com/cefsharp/CefSharp/issues/3470
        //Verify workaround passes
        public async Task CanCallCefSharpBindObjectAsyncWithoutParams()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl))
            {
                await browser.WaitForInitialLoadAsync();

                //TODO: See if we can avoid GetAwaiter().GetResult()
                var evt = Assert.Raises<JavascriptBindingEventArgs>(
                    x => browser.JavascriptObjectRepository.ResolveObject += x,
                    y => browser.JavascriptObjectRepository.ResolveObject -= y,
                    () => { browser.EvaluateScriptAsync("CefSharp.BindObjectAsync();").GetAwaiter().GetResult(); });

                Assert.NotNull(evt);

                Assert.Equal(JavascriptObjectRepository.AllObjects, evt.Arguments.ObjectName);
            }
        }
    }
}
