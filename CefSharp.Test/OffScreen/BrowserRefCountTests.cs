// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Internals;
using CefSharp.OffScreen;
using CefSharp.Web;
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
        public async Task ShouldWork()
        {
            var startCount = BrowserRefCounter.Instance.Count;

            using (var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);
                Assert.Equal(startCount + 1, BrowserRefCounter.Instance.Count);
            }

            Cef.WaitForBrowsersToClose(10000);

            output.WriteLine("BrowserRefCounter Log");
            output.WriteLine(BrowserRefCounter.Instance.GetLog());

            Assert.Equal(startCount, BrowserRefCounter.Instance.Count);
        }
    }
}
