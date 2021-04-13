// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Internals;
using CefSharp.OffScreen;
using CefSharp.Web;
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

        [Theory]
        [InlineData("lowerCustom")]
        [InlineData("UpperCustom")]
        public async Task CanEvaluateScriptAsPromiseAsyncJavascriptBindingApiGlobalObjectName(string rootObjName)
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com", automaticallyCreateBrowser:false))
            {
                browser.JavascriptObjectRepository.Settings.JavascriptBindingApiGlobalObjectName = rootObjName;
                browser.CreateBrowser();

                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsPromiseAsync("return new Promise(function(resolve, reject) { resolve(42); });");

                Assert.True(javascriptResponse.Success);

                Assert.Equal("42", javascriptResponse.Result.ToString());
            }
        }

        [Theory]
        [InlineData("return 42;", true, "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve(42); });", true, "42")]
        [InlineData("return new Promise(function(resolve, reject) { reject('reject test'); });", false, "reject test")]
        [InlineData("return await 42;", true, "42")]
        [InlineData("return await (function() { throw('reject test'); })();", false, "reject test")]
        [InlineData("var result = await fetch('./robots.txt'); return result.status;", true, "200")]
        public async Task CanEvaluateScriptAsPromiseAsync(string script, bool success, string expected)
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsPromiseAsync(script);

                Assert.Equal(success, javascriptResponse.Success);

                if (success)
                {
                    Assert.Equal(expected, javascriptResponse.Result.ToString());
                }
                else
                {
                    Assert.Equal(expected, javascriptResponse.Message);
                }
            }
        }

        [Theory]
        [InlineData("return { a: 'CefSharp', b: 42, };", true, "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve({ a: 'CefSharp', b: 42, }); });", true, "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });", true, "CefSharp", "42")]
        [InlineData("return await { a: 'CefSharp', b: 42, };", true, "CefSharp", "42")]
        [InlineData("return await new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); }); ", true, "CefSharp", "42")]
        public async Task CanEvaluateScriptAsPromiseAsyncReturnObject(string script, bool success, string expectedA, string expectedB)
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsPromiseAsync(script);

                Assert.Equal(success, javascriptResponse.Success);

                if (success)
                {
                    dynamic result = javascriptResponse.Result;
                    Assert.Equal(expectedA, result.a.ToString());
                    Assert.Equal(expectedB, result.b.ToString());
                }
                else
                {
                    throw new System.Exception("Failed");
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

        [Theory]
        [InlineData("http://httpbin.org/post")]
        public async Task CanLoadRequestWithPostData(string url)
        {
            const string data = "Testing123";
            //To use LoadRequest we must first load a web page
            using (var browser = new ChromiumWebBrowser(new HtmlString("Testing")))
            {
                await browser.LoadPageAsync();

                var request = new Request();
                request.Url = "http://httpbin.org/post";
                request.Method = "POST";
                var postData = new PostData();
                postData.AddElement(new PostDataElement
                {
                    Bytes = Encoding.UTF8.GetBytes(data)
                });

                request.PostData = postData;

                await browser.LoadRequestAsync(request);

                var mainFrame = browser.GetMainFrame();
                Assert.Equal(url, mainFrame.Url);

                var navEntry = await browser.GetVisibleNavigationEntryAsync();

                Assert.Equal((int)HttpStatusCode.OK, navEntry.HttpStatusCode);
                Assert.True(navEntry.HasPostData);

                var source = await browser.GetTextAsync();

                Assert.Contains(data, source);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task CanLoadHttpWebsiteUsingProxy()
        {
            fixture.StartProxyServerIfRequired();

            var requestContext = RequestContext
                .Configure()
                .WithProxyServer("127.0.0.1", 8080)
                .Create();
                
            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }
    }
}
