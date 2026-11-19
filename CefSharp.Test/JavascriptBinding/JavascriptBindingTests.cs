// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.Event;
using CefSharp.Example;
using CefSharp.Example.JavascriptBinding;
using CefSharp.Internals;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.JavascriptBinding
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavascriptBindingTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public JavascriptBindingTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            AssertInitialLoadComplete();

            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('bound');
                    return await bound.echo('Welcome to CefSharp!');
                })();";

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("bound", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("bound", boundObj, true);
#endif

            var result = await Browser.EvaluateScriptAsync<string>(script);

            Assert.Equal(1, boundObj.EchoMethodCallCount);
            Assert.Equal("Welcome to CefSharp!", result);
        }

        [Fact]
        //Issue https://github.com/cefsharp/CefSharp/issues/3470
        //Verify workaround passes
        public async Task ShouldRaiseResolveObjectEvent()
        {
            AssertInitialLoadComplete();

            var evt = await Assert.RaisesAsync<JavascriptBindingEventArgs>(
                x => Browser.JavascriptObjectRepository.ResolveObject += x,
                y => Browser.JavascriptObjectRepository.ResolveObject -= y,
                () => Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync();"));

            Assert.NotNull(evt);
            Assert.Equal(JavascriptObjectRepository.AllObjects, evt.Arguments.ObjectName);
        }

        [Fact]
        public async Task ShouldWorkWhenUsingCustomGlobalObjectName()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                browser.JavascriptObjectRepository.Settings.JavascriptBindingApiGlobalObjectName = "bindingApiObject";
                browser.CreateBrowser();
                await browser.WaitForInitialLoadAsync();
                var result = await browser.EvaluateScriptAsync("bindingApiObject.isObjectCached('doesntexist') === false");
                Assert.True(result.Success, result.Message);
            }
        }

        [Fact]
        public async Task ShouldWorkWhenUsingCustomGlobalObjectNameInMultipleBrowsers()
        {
            using var browser1 = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false);
            using var browser2 = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false);
            using var browser3 = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false);

            browser1.JavascriptObjectRepository.Settings.JavascriptBindingApiGlobalObjectName = "bindingApiObject1";
            browser2.JavascriptObjectRepository.Settings.JavascriptBindingApiGlobalObjectName = "bindingApiObject2";
            browser1.CreateBrowser();
            browser2.CreateBrowser();
            browser3.CreateBrowser();
            await browser1.WaitForInitialLoadAsync();
            await browser2.WaitForInitialLoadAsync();
            await browser3.WaitForInitialLoadAsync();

            Assert.False((bool)(await browser1.EvaluateScriptAsync("typeof window.bindingApiObject1 === 'undefined'")).Result);
            Assert.True((bool)(await browser1.EvaluateScriptAsync("typeof window.bindingApiObject2 === 'undefined'")).Result);
            Assert.True((bool)(await browser1.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'")).Result);
            Assert.True((bool)(await browser2.EvaluateScriptAsync("typeof window.bindingApiObject1 === 'undefined'")).Result);
            Assert.False((bool)(await browser2.EvaluateScriptAsync("typeof window.bindingApiObject2 === 'undefined'")).Result);
            Assert.True((bool)(await browser2.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'")).Result);
            Assert.True((bool)(await browser3.EvaluateScriptAsync("typeof window.bindingApiObject1 === 'undefined'")).Result);
            Assert.True((bool)(await browser3.EvaluateScriptAsync("typeof window.bindingApiObject2 === 'undefined'")).Result);
            Assert.False((bool)(await browser3.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'")).Result);
        }

        [Theory]
        [InlineData("CefSharp.IsObjectCached('doesntexist')")]
        [InlineData("cefSharp.isObjectCached('doesntexist')")]
        public async Task ShouldFailWhenIsObjectCachedCalledWithInvalidObjectName(string script)
        {
            var loadResponse = await Browser.LoadUrlAsync(CefExample.BindingApiCustomObjectNameTestUrl);
            Assert.True(loadResponse.Success);
            var response = await Browser.EvaluateScriptAsync(script);
            Assert.True(response.Success, response.Message);
            Assert.False((bool)response.Result);
        }

        [Fact]
        public async Task ShouldDisableJsBindingApi()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                browser.JavascriptObjectRepository.Settings.JavascriptBindingApiEnabled = false;
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.True((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.True((bool)response2.Result);
            }
        }

        [Theory]
        [InlineData("notallowed")]
        [InlineData("notallowed", "alsonotallowed")]
        [InlineData("notallowed", "alsonotallowed", "stillnotallowed")]
        public async Task ShouldDisableJsBindingApiForOrigin(params string[] origins)
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = origins;
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.True((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.True((bool)response2.Result);
            }
        }

        [Fact]
        public async Task ShouldEnableJsBindingApiWhenOriginsListIsEmpty()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = new string[0];
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.False((bool)response2.Result);
            }
        }

        [Theory]
        [InlineData(CefExample.BaseUrl)]
        [InlineData(CefExample.BaseUrl + "/")]
        public async Task ShouldEnableJsBindingApiForOriginWithOrWithoutTrailingSlash(string configuredOrigin)
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = new string[] { configuredOrigin };
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.False((bool)response2.Result);
            }
        }

        [Theory]
        [InlineData(CefExample.BaseUrl + "/")]
        [InlineData("someorigin", CefExample.BaseUrl + "/")]
        [InlineData(CefExample.BaseUrl + "/", "someorigin")]
        [InlineData("firstorigin", "secondorigin", CefExample.BaseUrl + "/")]
        [InlineData("firstorigin", CefExample.BaseUrl + "/", "secondorigin")]
        public async Task ShouldEnableJsBindingApiForOrigin(params string[] origins)
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = origins;
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.False((bool)response2.Result);
            }
        }

        [Fact]
        public async Task ShouldEnableJsBindingApi()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                browser.JavascriptObjectRepository.Settings.JavascriptBindingApiEnabled = true;
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.cefSharp === 'undefined'");
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                Assert.True(response2.Success, response2.Message);
                Assert.False((bool)response2.Result);
            }
        }

        [Theory]
        [InlineData("CefSharp.RenderProcessId")]
        [InlineData("cefSharp.renderProcessId")]
        public async Task ShouldReturnRenderProcessId(string script)
        {
            AssertInitialLoadComplete();
            var result = await Browser.EvaluateScriptAsync(script);
            Assert.True(result.Success, result.Message);
            using var process = Process.GetProcessById(Assert.IsType<int>(result.Result));
            Assert.Equal("CefSharp.BrowserSubprocess", process.ProcessName);
        }

        [Fact]
        public async Task ShouldWorkAfterACrossOriginNavigation()
        {
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('bound');
                    return await bound.echo('test');
                })();";
            var boundObj = new BindingTestObject();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("bound", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("bound", boundObj, true);
#endif
            await Browser.LoadUrlAsync("https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/url");
            await Browser.EvaluateScriptAsync<string>(script);
            await Browser.LoadUrlAsync("https://www.google.com");
            await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal(2, boundObj.EchoMethodCallCount);
        }

        [Fact]
        public async Task ShouldFireResolveObject()
        {
            AssertInitialLoadComplete();
            var objRepository = Browser.JavascriptObjectRepository;
            var evt = await Assert.RaisesAsync<JavascriptBindingEventArgs>(
                a => objRepository.ResolveObject += a,
                a => objRepository.ResolveObject -= a,
                () => Browser.EvaluateScriptAsync("(async function() { await CefSharp.BindObjectAsync('first'); })();"));
            Assert.NotNull(evt);
            Assert.Equal("first", evt.Arguments.ObjectName);
            Assert.Equal("https://cefsharp.example/HelloWorld.html", evt.Arguments.Url);
        }

        [Fact]
        public async Task ShouldFireResolveObjectForUnregisteredObject()
        {
            AssertInitialLoadComplete();
            var objRepository = Browser.JavascriptObjectRepository;
            var boundObj = new BindingTestObject();
#if NETCOREAPP
            objRepository.Register("first", boundObj);
#else
            objRepository.Register("first", boundObj, true);
#endif
            var evt = await Assert.RaisesAsync<JavascriptBindingEventArgs>(
                a => objRepository.ResolveObject += a,
                a => objRepository.ResolveObject -= a,
                () => Browser.EvaluateScriptAsync("(async function() { await CefSharp.BindObjectAsync('first', 'second'); })();"));
            Assert.NotNull(evt);
            Assert.Equal("second", evt.Arguments.ObjectName);
        }

        [Fact]
        public async Task ShouldDisableJsBindingApiAfterCrossOriginNavigationToDisallowedOrigin()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = new string[] { CefExample.BaseUrl };
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                var crossOriginLoad = await browser.LoadUrlAsync("https://www.google.com");
                Assert.True(crossOriginLoad.Success);
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response2.Success, response2.Message);
                Assert.True((bool)response2.Result);
            }
        }

        [Fact]
        public async Task ShouldKeepJsBindingApiEnabledAfterCrossOriginNavigationToAllowedOrigin()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiEnabled = true;
                settings.JavascriptBindingApiAllowOrigins = new string[] { CefExample.BaseUrl, "https://www.google.com" };
                browser.CreateBrowser();
                var loadResponse = await browser.WaitForInitialLoadAsync();
                Assert.True(loadResponse.Success);
                var response1 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response1.Success, response1.Message);
                Assert.False((bool)response1.Result);
                var crossOriginLoad = await browser.LoadUrlAsync("https://www.google.com");
                Assert.True(crossOriginLoad.Success);
                var response2 = await browser.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                Assert.True(response2.Success, response2.Message);
                Assert.False((bool)response2.Result);
            }
        }

        [Fact]
        public async Task ShouldEnforceAllowOriginsForPopup()
        {
            using (var requestContext = new RequestContext())
            {
                // Serve the popup from a local test scheme handler to avoid external network dependencies.
                requestContext.RegisterSchemeHandlerFactory("https", CefExample.ExampleDomain, new CefSharpSchemeHandlerFactory());
                requestContext.RegisterSchemeHandlerFactory("https", "cefsharp.com", new CefSharpSchemeHandlerFactory());

                using (var browser = new ChromiumWebBrowser(CefExample.BindingApiCustomObjectNameTestUrl, requestContext: requestContext, automaticallyCreateBrowser: false))
                {
                    browser.JavascriptObjectRepository.Settings.JavascriptBindingApiEnabled = true;
                    browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = new string[] { CefExample.BaseUrl };

                    var popupCreated = new TaskCompletionSource<ChromiumWebBrowser>(TaskCreationOptions.RunContinuationsAsynchronously);
                    browser.LifeSpanHandler = new PopupLifeSpanHandler(popupCreated, requestContext);
                    browser.CreateBrowser();

                    var loadResponse = await browser.WaitForInitialLoadAsync();
                    Assert.True(loadResponse.Success);

                    var setContent = await browser.SetMainFrameDocumentContentAsync(
                        "<html><body style='margin:0'><a href='https://cefsharp.com/HelloWorld.html' target='_blank' " +
                        "style='display:block;width:200px;height:40px'>Open popup</a></body></html>");
                    Assert.True(setContent);

                    var host = browser.GetBrowserHost();
                    host.SendMouseClickEvent(10, 10, MouseButtonType.Left, false, 1, CefEventFlags.None);
                    host.SendMouseClickEvent(10, 10, MouseButtonType.Left, true, 1, CefEventFlags.None);

                    var popupTask = popupCreated.Task;
                    var completedTask = await Task.WhenAny(popupTask, Task.Delay(10000));
                    Assert.Same(popupTask, completedTask);

                    var popup = await popupTask;
                    try
                    {
                        var popupLoadResponse = await popup.WaitForInitialLoadAsync();
                        Assert.True(popupLoadResponse.Success, "Popup load failed: " + popupLoadResponse.ErrorCode);

                        var bindingResponse = await popup.EvaluateScriptAsync("typeof window.CefSharp === 'undefined'");
                        Assert.True(bindingResponse.Success, bindingResponse.Message);
                        Assert.True((bool)bindingResponse.Result,
                            "The binding API must not be exposed to a popup whose origin is outside JavascriptBindingApiAllowOrigins.");
                    }
                    finally
                    {
                        popup.Dispose();
                    }
                }
            }
        }

        private sealed class PopupLifeSpanHandler : CefSharp.Handler.LifeSpanHandler
        {
            private readonly TaskCompletionSource<ChromiumWebBrowser> popupCreated;
            private readonly IRequestContext requestContext;

            public PopupLifeSpanHandler(TaskCompletionSource<ChromiumWebBrowser> popupCreated, IRequestContext requestContext)
            {
                this.popupCreated = popupCreated;
                this.requestContext = requestContext;
            }

            protected override bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
                string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture,
                IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings,
                ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                var popup = new ChromiumWebBrowser(targetUrl, requestContext: requestContext, automaticallyCreateBrowser: false);
                popup.JavascriptObjectRepository.Settings.JavascriptBindingApiEnabled = true;
                popup.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = new string[] { CefExample.BaseUrl };
                popup.SetAsPopup();
                newBrowser = popup;
                popupCreated.TrySetResult(popup);
                return false;
            }
        }
    }
}
