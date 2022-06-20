// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.DevTools.Page;
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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public async Task ShouldRespectDisposed()
        {
            ChromiumWebBrowser browser;

            using (browser = new ChromiumWebBrowser(CefExample.DefaultUrl))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Equal(CefExample.DefaultUrl, mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }

            Assert.True(browser.IsDisposed);

            Assert.Throws<ObjectDisposedException>(() =>
            {
                browser.Copy();
            });
        }

        [Fact]
        public async Task CanLoadInvalidDomain()
        {
            using (var browser = new ChromiumWebBrowser("notfound.cefsharp.test"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("notfound.cefsharp.test", mainFrame.Url);
                Assert.Equal(CefErrorCode.NameNotResolved, response.ErrorCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public async Task CanLoadExpiredBadSsl()
        {
            using (var browser = new ChromiumWebBrowser("https://expired.badssl.com/"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("", mainFrame.Url);
                Assert.Equal(-1, response.HttpStatusCode);
                Assert.Equal(CefErrorCode.CertDateInvalid, response.ErrorCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }        

        [Fact]
        public void BrowserRefCountDecrementedOnDispose()
        {
            var currentCount = BrowserRefCounter.Instance.Count;

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
            Assert.Equal(currentCount + 1, BrowserRefCounter.Instance.Count);

            browser.Dispose();

            Cef.WaitForBrowsersToClose(5000);

            output.WriteLine("BrowserRefCounter Log");
            output.WriteLine(BrowserRefCounter.Instance.GetLog());

            Assert.Equal(0, BrowserRefCounter.Instance.Count);
        }

        [Fact]
        public async Task CanCreateBrowserAsync()
        {
            using (var chromiumWebBrowser = new ChromiumWebBrowser("http://www.google.com", automaticallyCreateBrowser: false))
            {
                var browser = await chromiumWebBrowser.CreateBrowserAsync();

                Assert.NotNull(browser);
                Assert.False(browser.HasDocument);
                Assert.NotEqual(0, browser.Identifier);
                Assert.False(browser.IsDisposed);
            }
        }

        [Fact]
        public async Task CanLoadGoogleAndEvaluateScript()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

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
        public async Task CanEvaluateScriptInParallel()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(async () =>
                {
                    var javascriptResponse = await browser.EvaluateScriptAsync("2 + 2");

                    if (javascriptResponse.Success)
                    {
                        return (int)javascriptResponse.Result;
                    }

                    return -1;
                })).ToList();

                await Task.WhenAll(tasks);

                Assert.All(tasks, (t) =>
                {
                    Assert.Equal(4, t.Result);
                });
            }
        }

        [Theory]
        [InlineData("[1,2,,5]", new object[] { 1, 2, null, 5 })]
        [InlineData("[1,2,,]", new object[] { 1, 2, null })]
        [InlineData("[,2,3]", new object[] { null, 2, 3 })]
        [InlineData("[,2,,3,,4,,,,5,,,]", new object[] { null, 2, null, 3, null, 4, null, null, null, 5, null, null })]
        public async Task CanEvaluateScriptAsyncReturnPartiallyEmptyArrays(string javascript, object[] expected)
        {
            using (var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl))
            {
                await browser.WaitForInitialLoadAsync();

                var result = await browser.EvaluateScriptAsync(javascript);

                Assert.True(result.Success);
                Assert.Equal(expected, result.Result);
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

                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                browser.GetMainFrame().ExecuteJavaScriptAsync(script);

                await Task.Delay(2000);
                Assert.True(boundObj.MethodCalled);

                boundObj.MethodCalled = false;

                browser.Load("https://www.google.com");
                await browser.WaitForInitialLoadAsync();
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

                await browser.WaitForInitialLoadAsync();
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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

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

                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                var wasCached = false;
                var requestClient = new Example.UrlRequestClient((IUrlRequest req, byte[] responseBody) =>
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

        [Theory]
        [InlineData("https://code.jquery.com/jquery-3.4.1.min.js")]
        public async Task CanDownloadUrlForFrame(string url)
        {            
            using (var browser = new ChromiumWebBrowser(url))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var htmlSrc = await browser.GetSourceAsync();

                Assert.NotNull(htmlSrc);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var data = await mainFrame.DownloadUrlAsync(url);

                Assert.NotNull(data);
                Assert.True(data.Length > 0);

                var stringResult = Encoding.UTF8.GetString(data).Substring(0, 100);

                Assert.Contains(stringResult, htmlSrc);
            }
        }

        [Theory]
        [InlineData("https://code.jquery.com/jquery-3.4.1.min.js")]
        public async Task CanDownloadFileToFolderWithoutAskingUser(string url)
        {
            var tcs = new TaskCompletionSource<string>(TaskContinuationOptions.RunContinuationsAsynchronously);

            using (var chromiumWebBrowser = new ChromiumWebBrowser(url))
            {
                var userTempPath = System.IO.Path.GetTempPath();

                chromiumWebBrowser.DownloadHandler =
                    Fluent.DownloadHandler.UseFolder(userTempPath,
                        (chromiumBrowser, browser, downloadItem, callback) =>
                        {
                            if(downloadItem.IsComplete)
                            {
                                tcs.SetResult(downloadItem.FullPath);
                            }
                            else if(downloadItem.IsCancelled)
                            {
                                tcs.SetResult(null);
                            }
                        });

                await chromiumWebBrowser.WaitForInitialLoadAsync();

                chromiumWebBrowser.StartDownload(url);

                var downloadedFilePath = await tcs.Task;

                Assert.NotNull(downloadedFilePath);
                Assert.Contains(userTempPath, downloadedFilePath);
                Assert.True(System.IO.File.Exists(downloadedFilePath));

                var downloadedFileContent = System.IO.File.ReadAllText(downloadedFilePath);
                
                Assert.NotEqual(0, downloadedFileContent.Length);

                var htmlSrc = await chromiumWebBrowser.GetSourceAsync();

                Assert.Contains(downloadedFileContent.Substring(0, 100), htmlSrc);

                System.IO.File.Delete(downloadedFilePath);
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
                var requestClient = new Example.UrlRequestClient((IUrlRequest req, byte[] responseBody) =>
                {
                    statusCode = req.Response.StatusCode;
                    taskCompletionSource.TrySetResult(Encoding.UTF8.GetString(responseBody));
                });

                var request = new Request
                {
                    Method = "GET",
                    Url = "https://code.jquery.com/jquery-3.4.1.min.js"
                };

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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                Assert.True(browser.CanExecuteJavascriptInMainFrame);

                await browser.LoadUrlAsync(secondUrl);

                Assert.True(browser.CanExecuteJavascriptInMainFrame);

                await browser.LoadUrlAsync(firstUrl);

                Assert.True(browser.CanExecuteJavascriptInMainFrame);
            }
        }

        [Theory]
        [InlineData("http://httpbin.org/post")]
        public async Task CanLoadRequestWithPostData(string url)
        {
            const string data = "Testing123";
            //When Chromium Site Isolation is enabled we must first navigate to
            //a web page of the same origin to use LoadRequest
            //When Site Isolation is disabled we can navigate to any web page
            //https://magpcss.org/ceforum/viewtopic.php?f=10&t=18672&p=50266#p50249
            using (var browser = new ChromiumWebBrowser("http://httpbin.org/"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var request = new Request
                {
                    Url = "http://httpbin.org/post",
                    Method = "POST"
                };
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
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task CanLoadHttpWebsiteUsingSetProxyAsync()
        {
            fixture.StartProxyServerIfRequired();

            var tcs = new TaskCompletionSource<bool>();

            var requestContext = RequestContext
                .Configure()
                .OnInitialize((ctx) =>
                {
                    tcs.SetResult(true);
                })
                .Create();

            //Wait for our RequestContext to have initialized.
            await tcs.Task;

            var setProxyResponse = await requestContext.SetProxyAsync("127.0.0.1", 8080);

            Assert.True(setProxyResponse.Success);

            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task CanLoadHttpWebsiteUsingSetProxyOnUiThread()
        {
            fixture.StartProxyServerIfRequired();

            var tcs = new TaskCompletionSource<bool>();

            var requestContext = RequestContext
                .Configure()
                .OnInitialize((ctx) =>
                {
                    tcs.SetResult(true);
                })
                .Create();

            //Wait for our RequestContext to have initialized.
            await tcs.Task;

            var success = false;

            //To execute on the CEF UI Thread you can use 
            await Cef.UIThreadTaskFactory.StartNew(delegate
            {
                string errorMessage;

                if (!requestContext.CanSetPreference("proxy"))
                {
                    //Unable to set proxy, if you set proxy via command line args it cannot be modified.
                    success = false;

                    return;
                }

                success = requestContext.SetProxy("127.0.0.1", 8080, out errorMessage);
            });

            Assert.True(success);

            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public async Task CanWaitForBrowserInitialLoadAfterLoad()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google.com", mainFrame.Url);
                Assert.Equal(CefErrorCode.None, response.ErrorCode);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);

                response = await browser.WaitForInitialLoadAsync();

                Assert.Equal(CefErrorCode.None, response.ErrorCode);
                Assert.Equal(200, response.HttpStatusCode);
            }
        }

        [Fact]
        public async Task CanCallTryGetBrowserCoreByIdWithInvalidId()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var result = browser.TryGetBrowserCoreById(100, out IBrowser browserCore);

                Assert.False(result);
                Assert.Null(browserCore);
            }
        }

        [Fact]
        public async Task CanCallTryGetBrowserCoreByIdWithOwnId()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var result = browser.TryGetBrowserCoreById(browser.BrowserCore.Identifier, out IBrowser browserCore);

                Assert.True(result);
                Assert.NotNull(browserCore);
                Assert.Equal(browser.BrowserCore.Identifier, browserCore.Identifier);
            }
        }

        [Fact]
        public async Task CanCaptureScreenshotAsync()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var result1 = await browser.CaptureScreenshotAsync();
                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result1)))
                {
                    Assert.Equal(1366, screenshot.Width);
                    Assert.Equal(768, screenshot.Height);
                }


                var result2 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 1366, Height = 768, X = 100, Y = 200, Scale = 2 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result2)))
                {
                    Assert.Equal(2732, screenshot.Width);
                    Assert.Equal(1536, screenshot.Height);
                }


                var result3 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 100, Height = 200, Scale = 2 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result3)))
                {
                    Assert.Equal(200, screenshot.Width);
                    Assert.Equal(400, screenshot.Height);
                }


                var result4 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 100, Height = 200, Scale = 1 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result4)))
                {
                    Assert.Equal(100, screenshot.Width);
                    Assert.Equal(200, screenshot.Height);
                }
            }
        }

        [Fact]
        public async Task CanResizeWithDeviceScalingFactor()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);


                await browser.ResizeAsync(800, 600, 2);

                Assert.Equal(800, browser.Size.Width);
                Assert.Equal(600, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(1600, screenshot.Width);
                    Assert.Equal(1200, screenshot.Height);
                }


                await browser.ResizeAsync(400, 300);

                Assert.Equal(400, browser.Size.Width);
                Assert.Equal(300, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(800, screenshot.Width);
                    Assert.Equal(600, screenshot.Height);
                }


                await browser.ResizeAsync(1366, 768, 1);

                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(1366, screenshot.Width);
                    Assert.Equal(768, screenshot.Height);
                }
            }
        }

#if DEBUG
        [Fact]
        public async Task CanLoadMultipleBrowserInstancesSequentially()
        {
            for (int i = 0; i < 1000; i++)
            {
                using (var browser = new ChromiumWebBrowser(new HtmlString("Testing")))
                {
                    var response = await browser.WaitForInitialLoadAsync();

                    Assert.True(response.Success);

                    var source = await browser.GetSourceAsync();

                    Assert.Contains("Testing", source);
                }
            }
        }
#endif
    }
}
