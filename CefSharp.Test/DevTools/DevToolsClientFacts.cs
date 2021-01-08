// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.DevTools.Browser;
using CefSharp.DevTools.Network;
using CefSharp.Example;
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
        public async Task CanEmulationCanEmulate()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var response = await devToolsClient.Emulation.CanEmulateAsync();

                    Assert.True(response.Result);
                }
            }
        }

        [Fact]
        public async Task CanGetPageNavigationHistory()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var response = await devToolsClient.Page.GetNavigationHistoryAsync();
                    var currentIndex = response.CurrentIndex;
                    var entries = response.Entries;

                    Assert.Equal(0, currentIndex);
                    Assert.NotNull(entries);
                    Assert.True(entries.Count > 0);
                    Assert.Equal(CefSharp.DevTools.Page.TransitionType.Typed, entries[0].TransitionType);
                }
            }
        }

        [Theory]
        //[InlineData("CefSharpTest", "CefSharp Test Cookie", CefExample.ExampleDomain, CookieSameSite.None)]
        [InlineData("CefSharpTest1", "CefSharp Test Cookie2", CefExample.ExampleDomain, CookieSameSite.Lax)]
        public async Task CanSetCookieForDomain(string name, string value, string domain, CookieSameSite sameSite)
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var expiry = DateTimeOffset.UtcNow.AddDays(10);
                    var response = await devToolsClient.Network.SetCookieAsync(name, value, domain: domain, sameSite: sameSite, expires: expiry.ToUnixTimeSeconds());
                    Assert.True(response.Success, "SetCookieForDomain");
                }
            }
        }

        [Fact]
        public async Task CanUseMultipleDevToolsClientInstancesPerBrowser()
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
    }
}
