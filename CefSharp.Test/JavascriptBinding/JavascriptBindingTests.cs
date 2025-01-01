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
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JavascriptBindingApiGlobalObjectName = "bindingApiObject";

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                await browser.WaitForInitialLoadAsync();

                var result = await browser.EvaluateScriptAsync("bindingApiObject.isObjectCached('doesntexist') === false");

                Assert.True(result.Success);
            }
        }

        [Theory]
        //We'll execute twice using the different cased (camelcase naming and standard)
        [InlineData("CefSharp.IsObjectCached('doesntexist')")]
        [InlineData("cefSharp.isObjectCached('doesntexist')")]
        public async Task ShouldFailWhenIsObjectCachedCalledWithInvalidObjectName(string script)
        {
            var loadResponse = await Browser.LoadUrlAsync(CefExample.BindingApiCustomObjectNameTestUrl);

            Assert.True(loadResponse.Success);
            
            var response = await Browser.EvaluateScriptAsync(script);

            Assert.True(response.Success);
            Assert.False((bool)response.Result);
        }

        [Fact]
        public async Task ShouldDisableJsBindingApi()
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
        public async Task ShouldEnableJsBindingApi()
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
        public async Task ShouldReturnRenderProcessId(string script)
        {
            AssertInitialLoadComplete();

            var result = await Browser.EvaluateScriptAsync(script);

            Assert.True(result.Success);

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
    }
}
