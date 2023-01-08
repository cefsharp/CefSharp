// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using System.Windows;
using CefSharp.Example;
using CefSharp.Wpf;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Wpf
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    [BrowserRefCountDebugging(typeof(ChromiumWebBrowser))]
    public class WpfBrowserBasicFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WpfBrowserBasicFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [WpfFact]
        public async Task CanLoadGoogle()
        {
            using (var browser = new ChromiumWebBrowser(null, "www.google.com", new Size(1024, 786)))
            {
                await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task CanCallLoadUrlAsyncImmediately()
        {
            using (var browser = new ChromiumWebBrowser(null, string.Empty, new Size(1024, 786)))
            {
                var response = await browser.LoadUrlAsync("www.google.com");

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task CanCallLoadUrlImmediately()
        {
            using (var browser = new ChromiumWebBrowser())
            {
                browser.Load("www.google.com");
                browser.CreateBrowser(null, new Size(1024, 786));

                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task CanSetRequestContext()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                browser.RequestContext = new RequestContext();
                browser.CreateBrowser(null, new Size(1024, 786));

                await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task CanSetRequestContextViaBuilder()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                browser.RequestContext = RequestContext.Configure()
                    .WithSharedSettings(Cef.GetGlobalRequestContext())
                    .Create();

                browser.CreateBrowser(null, new Size(1024, 786));

                await browser.WaitForInitialLoadAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WpfFact]
        public async Task ShouldRespectDisposed()
        {
            ChromiumWebBrowser browser;

            using (browser = new ChromiumWebBrowser(null, CefExample.DefaultUrl, new Size(1024, 786)))
            {
                await browser.WaitForInitialLoadAsync();
            }

            Assert.True(browser.IsDisposed);

            var ex = Assert.Throws<ObjectDisposedException>(() =>
            {
                browser.Copy();
            });
        }
    }
}
