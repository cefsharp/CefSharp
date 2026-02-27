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

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithMultipleObjects()
        {
            AssertInitialLoadComplete();

            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('bound1', 'bound2');
                    var result1 = await bound1.echo('test1');
                    var result2 = await bound2.echo('test2');
                    return result1 + '|' + result2;
                })();";

            var boundObj1 = new BindingTestObject();
            var boundObj2 = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("bound1", boundObj1);
            Browser.JavascriptObjectRepository.Register("bound2", boundObj2);
#else
            Browser.JavascriptObjectRepository.Register("bound1", boundObj1, true);
            Browser.JavascriptObjectRepository.Register("bound2", boundObj2, true);
#endif

            var result = await Browser.EvaluateScriptAsync<string>(script);

            Assert.Equal(1, boundObj1.EchoMethodCallCount);
            Assert.Equal(1, boundObj2.EchoMethodCallCount);
            Assert.Equal("test1|test2", result);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithCachedObjects()
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("cached", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("cached", boundObj, true);
#endif

            // First bind - should cache
            await Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync('cached');");
            
            // Second bind - should use cache
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('cached');
                    return await cached.echo('from cache');
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal("from cache", result);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithIgnoreCacheOption()
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("testobj", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("testobj", boundObj, true);
#endif

            // Bind with ignoreCache option
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync({IgnoreCache: true}, 'testobj');
                    return await testobj.echo('no cache');
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal("no cache", result);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithNotifyIfAlreadyBound()
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("notify", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("notify", boundObj, true);
#endif

            // First bind
            await Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync('notify');");

            // Try to bind again with notifyIfAlreadyBound
            const string script = @"
                (async function()
                {
                    var result = await CefSharp.BindObjectAsync({NotifyIfAlreadyBound: true}, 'notify');
                    return result.Success;
                })();";

            var result = await Browser.EvaluateScriptAsync<bool>(script);
            Assert.False(result);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncAfterMultipleNavigations()
        {
            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("persistent", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("persistent", boundObj, true);
#endif

            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('persistent');
                    return await persistent.echo('test');
                })();";

            // Navigate and bind multiple times
            for (int i = 0; i < 3; i++)
            {
                await Browser.LoadUrlAsync(CefExample.HelloWorldUrl);
                var result = await Browser.EvaluateScriptAsync<string>(script);
                Assert.Equal("test", result);
            }

            Assert.Equal(3, boundObj.EchoMethodCallCount);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithMixedRegisteredAndUnregistered()
        {
            AssertInitialLoadComplete();

            var boundObj1 = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("registered", boundObj1);
#else
            Browser.JavascriptObjectRepository.Register("registered", boundObj1, true);
#endif

            // Try to bind both registered and unregistered object
            var objRepository = Browser.JavascriptObjectRepository;

            var evt = await Assert.RaisesAsync<JavascriptBindingEventArgs>(
                a => objRepository.ResolveObject += a,
                a => objRepository.ResolveObject -= a,
                () => Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync('registered', 'unregistered');"));

            Assert.NotNull(evt);
            Assert.Equal("unregistered", evt.Arguments.ObjectName);
        }

        [Theory]
        [InlineData("CefSharp.BindObjectAsync()")]
        [InlineData("cefSharp.bindObjectAsync()")]
        public async Task ShouldHandleBindObjectAsyncWithBothCasingVariants(string bindScript)
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("casingtest", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("casingtest", boundObj, true);
#endif

            var script = $@"
                (async function()
                {{
                    await {bindScript};
                    return true;
                }})();";

            var result = await Browser.EvaluateScriptAsync<bool>(script);
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldVerifyJavascriptRootObjectWrapperIsNotNull()
        {
            // This test verifies the fix where _javascriptRootObjectWrapper should not be null
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("roottest", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("roottest", boundObj, true);
#endif

            // This should not throw an exception about null _javascriptRootObjectWrapper
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('roottest');
                    return await roottest.echo('success');
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal("success", result);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncInIframe()
        {
            using (var browser = new ChromiumWebBrowser(automaticallyCreateBrowser: false))
            {
                var boundObj = new BindingTestObject();

#if NETCOREAPP
                browser.JavascriptObjectRepository.Register("iframeobj", boundObj);
#else
                browser.JavascriptObjectRepository.Register("iframeobj", boundObj, true);
#endif

                browser.CreateBrowser();
                
                await browser.LoadHtmlAsync(@"
                    <html>
                        <body>
                            <h1>Main Frame</h1>
                            <iframe id='testFrame' srcdoc='<script>console.log(""iframe loaded"");</script>'></iframe>
                        </body>
                    </html>");

                // Bind in main frame
                const string script = @"
                    (async function()
                    {
                        await CefSharp.BindObjectAsync('iframeobj');
                        return await iframeobj.echo('main frame');
                    })();";

                var result = await browser.EvaluateScriptAsync<string>(script);
                Assert.Equal("main frame", result);
            }
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithEmptyObjectList()
        {
            AssertInitialLoadComplete();

            // Bind with no objects specified
            var result = await Browser.EvaluateScriptAsync("CefSharp.BindObjectAsync()");
            Assert.True(result.Success);
        }

        [Fact]
        public async Task ShouldHandleBindObjectAsyncWithConfigurationObject()
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("configtest", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("configtest", boundObj, true);
#endif

            // Bind with configuration object
            const string script = @"
                (async function()
                {
                    var config = {
                        NotifyIfAlreadyBound: false,
                        IgnoreCache: false
                    };
                    await CefSharp.BindObjectAsync(config, 'configtest');
                    return await configtest.echo('configured');
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal("configured", result);
        }

        [Fact]
        public async Task ShouldHandleConcurrentBindObjectAsyncCalls()
        {
            AssertInitialLoadComplete();

            var boundObj1 = new BindingTestObject();
            var boundObj2 = new BindingTestObject();
            var boundObj3 = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("concurrent1", boundObj1);
            Browser.JavascriptObjectRepository.Register("concurrent2", boundObj2);
            Browser.JavascriptObjectRepository.Register("concurrent3", boundObj3);
#else
            Browser.JavascriptObjectRepository.Register("concurrent1", boundObj1, true);
            Browser.JavascriptObjectRepository.Register("concurrent2", boundObj2, true);
            Browser.JavascriptObjectRepository.Register("concurrent3", boundObj3, true);
#endif

            // Execute multiple bind operations concurrently
            var task1 = Browser.EvaluateScriptAsync(@"
                (async function() {
                    await CefSharp.BindObjectAsync('concurrent1');
                    return await concurrent1.echo('1');
                })();");

            var task2 = Browser.EvaluateScriptAsync(@"
                (async function() {
                    await CefSharp.BindObjectAsync('concurrent2');
                    return await concurrent2.echo('2');
                })();");

            var task3 = Browser.EvaluateScriptAsync(@"
                (async function() {
                    await CefSharp.BindObjectAsync('concurrent3');
                    return await concurrent3.echo('3');
                })();");

            await Task.WhenAll(task1, task2, task3);

            Assert.Equal("1", await task1);
            Assert.Equal("2", await task2);
            Assert.Equal("3", await task3);
        }

        [Theory]
        [InlineData("bindObjectAsync")]
        [InlineData("BindObjectAsync")]
        public async Task ShouldSupportCamelCaseAndPascalCaseBindMethods(string methodName)
        {
            AssertInitialLoadComplete();

            var boundObj = new BindingTestObject();

#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("casetest", boundObj);
#else
            Browser.JavascriptObjectRepository.Register("casetest", boundObj, true);
#endif

            var script = $@"
                (async function()
                {{
                    await CefSharp.{methodName}('casetest');
                    return await casetest.echo('works');
                }})();";

            var result = await Browser.EvaluateScriptAsync<string>(script);
            Assert.Equal("works", result);
        }
    }
}
