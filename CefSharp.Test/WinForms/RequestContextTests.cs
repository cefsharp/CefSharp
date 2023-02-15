// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.WinForms;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.WinForms
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

        [WinFormsFact]
        public async Task ShouldWork()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                browser.RequestContext = new RequestContext();

                browser.Size = new System.Drawing.Size(1024, 768);
                browser.CreateControl();

                await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [WinFormsFact]
        public async Task ShouldWorkUsingBuilder()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                browser.RequestContext = RequestContext.Configure()
                    .WithSharedSettings(Cef.GetGlobalRequestContext())
                    .Create();

                browser.Size = new System.Drawing.Size(1024, 768);
                browser.CreateControl();

                await browser.WaitForInitialLoadAsync();
                var mainFrame = browser.GetMainFrame();

                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }
    }
}
