// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class RequestContextTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public RequestContextTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task ShouldWorkWithProxy()
        {
            fixture.StartProxyServerIfRequired();

            var requestContext = RequestContext
                .Configure()
                .WithProxyServer("127.0.0.1", 8080)
                .Create();

            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task ShouldWorkWithSetProxyAsync()
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

            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task ShouldWorkWithProxySetOnUiThread()
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

            using (var browser = new ChromiumWebBrowser("http://cefsharp.github.io/", requestContext: requestContext, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("cefsharp.github.io", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }
    }
}
