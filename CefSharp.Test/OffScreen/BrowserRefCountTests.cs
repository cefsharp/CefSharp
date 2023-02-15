// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading;
using CefSharp.Internals;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class BrowserRefCountTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public BrowserRefCountTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }
    
        [Fact]
        public void ShouldWork()
        {
            var currentCount = BrowserRefCounter.Instance.Count;

            var manualResetEvent = new ManualResetEvent(false);

            var browser = new ChromiumWebBrowser("https://google.com", useLegacyRenderHandler: false);

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

            Cef.WaitForBrowsersToClose(10000);

            output.WriteLine("BrowserRefCounter Log");
            output.WriteLine(BrowserRefCounter.Instance.GetLog());

            Assert.Equal(0, BrowserRefCounter.Instance.Count);
        }
    }
}
