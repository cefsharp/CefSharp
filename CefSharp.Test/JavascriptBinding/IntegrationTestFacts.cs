// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Example.JavascriptBinding;
using CefSharp.OffScreen;
using CefSharp.Web;
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

                browser.CreateBrowser();
                var success = await browser.WaitForQUnitTestExeuctionToComplete();

                Assert.True(success);

                output.WriteLine("QUnit Tests result: {0}", success);
            }
        }
#else
        [Fact]
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

                browser.CreateBrowser();
                var success = await browser.WaitForQUnitTestExeuctionToComplete();

                Assert.True(success);

                output.WriteLine("QUnit Tests result: {0}", success);
            }
        }
#endif

        [Fact]
        public async Task IsObjectCachedWithInvalidObjectNameReturnsFalse()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl))
            {
                await browser.LoadPageAsync();

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
                settings.JavascriptBindingApiGlobalObjectName = "customApi";

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                await browser.LoadPageAsync();

                var result = await browser.EvaluateScriptAsync("customApi.isObjectCached('doesntexist') === false");

                Assert.True(result.Success);
            }
        }

        [Theory]
        [InlineData("Event", "Event1")]
        [InlineData("Event", "Event2")]
        [InlineData("CustomEvent", "Event1")]
        [InlineData("CustomEvent", "Event2")]
        public async Task JavascriptsEventsReceived(string jsEventObject, string eventToRaise)
        {
            using (var cancelSource = new CancellationTokenSource())
            using (var events = new BlockingCollection<string>())
            using (var browser = new JavascriptTestWebBrowser())
            {
                cancelSource.CancelAfter(10_000);
                while (!browser.IsBrowserInitialized)
                {
                    await Task.Delay(50);
                }

                browser.PageEvent += (o, e) =>
                {
                    try
                    {
                        events.Add(e);
                    }
                    catch { }
                };
                string rawHtml = $"<html><head><script>window.dispatchEvent(new {jsEventObject}(\"{eventToRaise}\"));</script></head><body><h1>testing</h1></body></html>";
                var html = new HtmlString(rawHtml, true);
                browser.Load(html.ToDataUriString());
                string eventName = null;
                try
                {
                    eventName = events.Take(cancelSource.Token);
                }
                catch
                {
                    Assert.True(false, "did not receive event");
                }

                Assert.Equal(eventToRaise, eventName);
            }
        }
    }
}
