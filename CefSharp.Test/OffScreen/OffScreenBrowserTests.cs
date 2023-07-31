// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using CefSharp.Web;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class OffScreenBrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public OffScreenBrowserTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWorkWhenLoadingGoogle()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com", useLegacyRenderHandler:false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Theory]
        [InlineData("http://httpbin.org/post")]
        public async Task ShouldWorkWhenLoadingRequestWithPostData(string url)
        {
            const string data = "Testing123";
            //When Chromium Site Isolation is enabled we must first navigate to
            //a web page of the same origin to use LoadRequest
            //When Site Isolation is disabled we can navigate to any web page
            //https://magpcss.org/ceforum/viewtopic.php?f=10&t=18672&p=50266#p50249
            using (var browser = new ChromiumWebBrowser("http://httpbin.org/", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success, $"Initial Load Error Code: {response.ErrorCode.ToString()} Http Status Code: {response.HttpStatusCode}");

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
                Assert.True(navEntry.HasPostData, "Has PostData");

                var source = await browser.GetTextAsync();

                Assert.Contains(data, source);
            }
        }

        [Fact]
        public async Task ShouldFailWhenLoadingInvalidDomain()
        {
            using (var browser = new ChromiumWebBrowser("notfound.cefsharp.test", useLegacyRenderHandler: false))
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
        public async Task ShouldFailWhenLoadingBadSsl()
        {
            using (var browser = new ChromiumWebBrowser("https://expired.badssl.com/", useLegacyRenderHandler: false))
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
        public async Task ShouldRespectDisposed()
        {
            ChromiumWebBrowser browser;

            using (browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Equal(CefExample.DefaultUrl, mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }

            Assert.True(browser.IsDisposed, $"Browser IsDisposed:{browser.IsDisposed}");

            Assert.Throws<ObjectDisposedException>(() =>
            {
                browser.Copy();
            });
        }

        [Fact]
        public async Task ShouldWorkWhenBrowserCreatedAsync()
        {
            using (var chromiumWebBrowser = new ChromiumWebBrowser("http://www.google.com", automaticallyCreateBrowser: false, useLegacyRenderHandler: false))
            {
                var browser = await chromiumWebBrowser.CreateBrowserAsync();

                Assert.NotNull(browser);
                Assert.False(browser.HasDocument);
                Assert.NotEqual(0, browser.Identifier);
                Assert.False(browser.IsDisposed);
            }
        }

        [Fact]
        public async Task ShouldMakeFrameUrlRequest()
        {
            using (var browser = new ChromiumWebBrowser("https://code.jquery.com/jquery-3.4.1.min.js", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
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
        public async Task ShouldDownloadUrlForFrame(string url)
        {            
            using (var browser = new ChromiumWebBrowser(url, useLegacyRenderHandler: false))
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
        //TODO: Add more urls
        [InlineData("http://www.google.com", "http://cefsharp.github.io/")]
        public async Task CanExecuteJavascriptInMainFrameAfterNavigatingToDifferentOrigin(string firstUrl, string secondUrl)
        {
            using (var browser = new ChromiumWebBrowser(firstUrl, useLegacyRenderHandler: false))
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

        [Fact]
        public async Task ShouldWaitForBrowserInitialLoadAfterSubsequentLoad()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com", useLegacyRenderHandler: false))
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
        public async Task ShouldFailWhenCallingTryGetBrowserCoreByIdWithInvalidId()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var result = browser.TryGetBrowserCoreById(100, out IBrowser browserCore);

                Assert.False(result);
                Assert.Null(browserCore);
            }
        }

        [Fact]
        public async Task ShouldWorkWhenCallingTryGetBrowserCoreByIdWithOwnId()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var result = browser.TryGetBrowserCoreById(browser.BrowserCore.Identifier, out IBrowser browserCore);

                Assert.True(result);
                Assert.NotNull(browserCore);
                Assert.Equal(browser.BrowserCore.Identifier, browserCore.Identifier);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task ShouldWorkWhenCreatingOneThousandBrowserSequentially()
        {
            for (int i = 0; i < 1000; i++)
            {
                using (var browser = new ChromiumWebBrowser(new HtmlString("Testing"), useLegacyRenderHandler: false))
                {
                    var response = await browser.WaitForInitialLoadAsync();

                    Assert.True(response.Success);

                    var source = await browser.GetSourceAsync();

                    Assert.Contains("Testing", source);
                }
            }
        }
    }
}
