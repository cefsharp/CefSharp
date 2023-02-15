// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Example.JavascriptBinding;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.JavascriptBinding
{
    /// <summary>
    /// Automated QUnit Integration Tests 
    /// </summary>
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class IntegrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public IntegrationTests(ITestOutputHelper output, CefSharpFixture fixture)
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

        // Issue https://github.com/cefsharp/CefSharp/issues/3867
        [SkipIfRunOnAppVeyorFact]
        public async Task LoadJavaScriptBindingQunitTestsSuccessfulCompletion()
        {
            var requestContext = new RequestContext();
            requestContext.RegisterSchemeHandlerFactory("https", CefExample.ExampleDomain, new CefSharpSchemeHandlerFactory());

            using (var browser = new ChromiumWebBrowser(CefExample.BindingTestUrl, requestContext: requestContext, automaticallyCreateBrowser: false))
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
            var requestContext = new RequestContext();
            requestContext.RegisterSchemeHandlerFactory("https", CefExample.ExampleDomain, new CefSharpSchemeHandlerFactory());

            using (var browser = new ChromiumWebBrowser(CefExample.LegacyBindingTestUrl, requestContext:requestContext, automaticallyCreateBrowser: false))
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
    }
}
