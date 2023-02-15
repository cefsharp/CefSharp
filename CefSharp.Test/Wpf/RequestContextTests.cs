// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using System.Windows;
using CefSharp.Wpf;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Wpf
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    [BrowserRefCountDebugging(typeof(ChromiumWebBrowser))]
    public class RequestContextTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public RequestContextTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [WpfFact]
        public async Task ShouldWork()
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
        public async Task ShouldWorkUsingBuilder()
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
    }
}
