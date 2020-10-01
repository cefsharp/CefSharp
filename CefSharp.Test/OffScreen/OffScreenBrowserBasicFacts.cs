// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Internals;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class OffScreenBrowserBasicFacts
    {
        //TODO: Move into own file/namespace
        public class AsyncBoundObject
        {
            public bool MethodCalled { get; set; }
            public string Echo(string arg)
            {
                MethodCalled = true;
                return arg;
            }
        }

        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public OffScreenBrowserBasicFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task CanLoadGoogle()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public void BrowserRefCountDecrementedOnDispose()
        {
            var manualResetEvent = new ManualResetEvent(false);

            var browser = new ChromiumWebBrowser("https://google.com");
            browser.LoadingStateChanged += (sender, e) =>
            {
                if (!e.IsLoading)
                {
                    manualResetEvent.Set();
                }
            };

            manualResetEvent.WaitOne();

            //TODO: Refactor this so reference is injected into browser
            Assert.Equal(1, BrowserRefCounter.Instance.Count);

            browser.Dispose();

            Assert.True(BrowserRefCounter.Instance.Count <= 1);

            Cef.WaitForBrowsersToClose();

            Assert.Equal(0, BrowserRefCounter.Instance.Count);
        }

        [Fact]
        public async Task CanLoadGoogleAndEvaluateScript()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                var javascriptResponse = await browser.EvaluateScriptAsync("2 + 2");
                Assert.True(javascriptResponse.Success);
                Assert.Equal(4, (int)javascriptResponse.Result);
                output.WriteLine("Result of 2 + 2: {0}", javascriptResponse.Result);
            }
        }

        [Fact]
        public async Task CrossSiteNavigationJavascriptBinding()
        {
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('bound');
                    bound.echo('test');
                })();";

            var boundObj = new AsyncBoundObject();

            using (var browser = new ChromiumWebBrowser("https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/url"))
            {
#if NETCOREAPP
                browser.JavascriptObjectRepository.Register("bound", boundObj);
#else
                browser.JavascriptObjectRepository.Register("bound", boundObj, true);
#endif

                await browser.LoadPageAsync();
                browser.GetMainFrame().ExecuteJavaScriptAsync(script);

                await Task.Delay(2000);
                Assert.True(boundObj.MethodCalled);

                boundObj.MethodCalled = false;

                browser.Load("https://www.google.com");
                await browser.LoadPageAsync();
                browser.GetMainFrame().ExecuteJavaScriptAsync(script);
                await Task.Delay(2000);
                Assert.True(boundObj.MethodCalled);
            }
        }

        [Fact]
        public async Task JavascriptBindingMultipleObjects()
        {
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('first');
                    await CefSharp.BindObjectAsync('first', 'second');
                })();";

            var objectNames = new List<string>();
            var boundObj = new AsyncBoundObject();

            using (var browser = new ChromiumWebBrowser("https://www.google.com"))
            {
                browser.JavascriptObjectRepository.ResolveObject += (s, e) =>
                {
                    objectNames.Add(e.ObjectName);
#if NETCOREAPP
                    e.ObjectRepository.Register(e.ObjectName, boundObj);
#else
                    e.ObjectRepository.Register(e.ObjectName, boundObj, isAsync: true);
#endif
                };

                await browser.LoadPageAsync();
                browser.GetMainFrame().ExecuteJavaScriptAsync(script);

                await Task.Delay(2000);
                Assert.Equal(new[] { "first", "second" }, objectNames);
            }
        }

        /// <summary>
        /// Use the EvaluateScriptAsync (IWebBrowser, String,Object[]) overload and pass in string params
        /// that require encoding. Test case for https://github.com/cefsharp/CefSharp/issues/2339
        /// </summary>
        /// <returns>A task</returns>
        [Fact]
        public async Task CanEvaluateScriptAsyncWithEncodedStringArguments()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsync("var testfunc=function(s) { return s; }");
                Assert.True(javascriptResponse.Success);

                // now call the function we just created
                string[] teststrings = new string[]{"Mary's\tLamb & \r\nOther Things",
                                      "[{test:\"Mary's Lamb & \\nOther Things\", 'other': \"\", 'and': null}]" };
                foreach (var test in teststrings)
                {
                    javascriptResponse = await browser.EvaluateScriptAsync("testfunc", test);
                    Assert.True(javascriptResponse.Success);
                    Assert.Equal(test, (string)javascriptResponse.Result);
                    output.WriteLine("{0} passes {1}", test, javascriptResponse.Result);
                }
            }
        }

        [Fact]
        public async Task CanMakeFrameUrlRequest()
        {
            using (var browser = new ChromiumWebBrowser("https://code.jquery.com/jquery-3.4.1.min.js"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                var wasCached = false;
                var requestClient = new UrlRequestClient((IUrlRequest req, byte[] responseBody) =>
                {
                    wasCached = req.ResponseWasCached;
                    taskCompletionSource.TrySetResult(Encoding.UTF8.GetString(responseBody));
                });

                //Can be created on any valid CEF Thread, here we'll use the CEF UI Thread
                await Cef.UIThreadTaskFactory.StartNew(delegate
                {
                    var request = mainFrame.CreateRequest(false);

                    request.Method = "GET";
                    request.Url = "https://code.jquery.com/jquery-3.4.1.min.js";
                    var urlRequest = mainFrame.CreateUrlRequest(request, requestClient);
                });

                var stringResult = await taskCompletionSource.Task;

                Assert.True(!string.IsNullOrEmpty(stringResult));
                Assert.True(wasCached);
            }
        }

        [Fact]
        public async Task CanMakeUrlRequest()
        {
            var taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            IUrlRequest urlRequest = null;
            int statusCode = -1;

            //Can be created on any valid CEF Thread, here we'll use the CEF UI Thread
            await Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var requestClient = new UrlRequestClient((IUrlRequest req, byte[] responseBody) =>
                {
                    statusCode = req.Response.StatusCode;
                    taskCompletionSource.TrySetResult(Encoding.UTF8.GetString(responseBody));
                });

                var request = new Request();
                request.Method = "GET";
                request.Url = "https://code.jquery.com/jquery-3.4.1.min.js";

                //Global RequestContext will be used
                urlRequest = new UrlRequest(request, requestClient);
            });

            var stringResult = await taskCompletionSource.Task;

            Assert.True(!string.IsNullOrEmpty(stringResult));
            Assert.Equal(200, statusCode);
        }

        [Theory]
        //TODO: Add more urls
        [InlineData("http://www.google.com", "http://cefsharp.github.io/")]
        public async Task CanExecuteJavascriptInMainFrameAfterNavigatingToDifferentOrigin(string firstUrl, string secondUrl)
        {
            using (var browser = new ChromiumWebBrowser(firstUrl))
            {
                await browser.LoadPageAsync();

                Assert.True(browser.CanExecuteJavascriptInMainFrame);

                await browser.LoadPageAsync(secondUrl);

                Assert.True(browser.CanExecuteJavascriptInMainFrame);

                await browser.LoadPageAsync(firstUrl);

                Assert.True(browser.CanExecuteJavascriptInMainFrame);
            }
        }
    }
}
