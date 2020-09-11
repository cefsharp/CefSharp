// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.DevTools.Browser;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.DevTools
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class DevToolsClientFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public DevToolsClientFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public void CanConvertDevToolsObjectToDictionary()
        {
            var bounds = new Bounds
            {
                Height = 1,
                Width = 1,
                Left = 1,
                Top = 1,
                WindowState = WindowState.Fullscreen
            };

            var dict = bounds.ToDictionary();

            Assert.Equal(bounds.Height, (int)dict["height"]);
            Assert.Equal(bounds.Width, (int)dict["width"]);
            Assert.Equal(bounds.Top, (int)dict["top"]);
            Assert.Equal(bounds.Left, (int)dict["left"]);
            Assert.Equal("fullscreen", (string)dict["windowState"]);
        }


        [Fact]
        public async Task CanGetDevToolsProtocolVersion()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var response = await devToolsClient.Browser.GetVersionAsync();
                    var jsVersion = response.JsVersion;
                    var revision = response.Revision;

                    Assert.NotNull(jsVersion);
                    Assert.NotNull(revision);

                    output.WriteLine("DevTools Revision {0}", revision);
                }
            }
        }

        [Fact]
        public async Task CanGetDevToolsProtocolGetWindowForTarget()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var response = await devToolsClient.Browser.GetWindowForTargetAsync();
                    var windowId = response.WindowId;
                    var bounds = response.Bounds;

                    Assert.NotEqual(0, windowId);
                    Assert.NotNull(bounds);
                    Assert.True(bounds.Height > 0);
                    Assert.True(bounds.Width > 0);
                    Assert.True(bounds.WindowState.HasValue);
                }
            }
        }

    }
}
