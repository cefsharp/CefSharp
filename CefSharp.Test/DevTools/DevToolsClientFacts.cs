// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.DevTools.Browser;
using CefSharp.DevTools.Emulation;
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
                await browser.LoadUrlAsync();

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
                await browser.LoadUrlAsync();

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
                await browser.LoadUrlAsync();

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
                await browser.LoadUrlAsync();

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
                await browser.LoadUrlAsync();

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

        [Fact]
        public async Task CanSetUserAgentOverride()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadUrlAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var brandsList = new List<UserAgentBrandVersion>();
                    var uab = new UserAgentBrandVersion();
                    uab.Brand = "Google Chrome";
                    uab.Version = "89";
                    brandsList.Add(uab);

                    var uab2 = new UserAgentBrandVersion();
                    uab2.Brand = "Chromium";
                    uab2.Version = "89";
                    brandsList.Add(uab2);

                    var ua = new UserAgentMetadata();
                    ua.Brands = brandsList;
                    ua.Architecture = "arm";
                    ua.Model = "Nexus 7";
                    ua.Platform = "Android";
                    ua.PlatformVersion = "6.0.1";
                    ua.FullVersion = "89.0.4389.114";
                    ua.Mobile = true;

                    await devToolsClient.Emulation.SetUserAgentOverrideAsync("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 7 Build/MOB30X) AppleWebKit/5(KHTML,likeGeckoChrome/89.0.4389.114Safari/537.36", null, null, ua);
                }

                var userAgent = await browser.EvaluateScriptAsync("navigator.userAgent");
                Assert.True(userAgent.Success);
                Assert.Contains("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 7 Build/MOB30X) AppleWebKit/5(KHTML,likeGeckoChrome/89.0.4389.114Safari/537.36", Assert.IsType<string>(userAgent.Result));

                var brands = await browser.EvaluateScriptAsync("navigator.userAgentData.brands");
                Assert.True(brands.Success);
                dynamic brandsResult = brands.Result;
                Assert.Collection((IEnumerable<dynamic>)brandsResult,
                    (dynamic d) =>
                    {
                        Assert.Equal("Google Chrome", d.brand);
                        Assert.Equal("89", d.version);
                    },
                    (dynamic d) =>
                    {
                        Assert.Equal("Chromium", d.brand);
                        Assert.Equal("89", d.version);
                    }
                );

                var highEntropyValues = await browser.EvaluateScriptAsPromiseAsync("return navigator.userAgentData.getHighEntropyValues(['architecture','model','platform','platformVersion','uaFullVersion'])");
                Assert.True(highEntropyValues.Success);
                dynamic highEntropyValuesResult = highEntropyValues.Result;
                Assert.Equal("arm", highEntropyValuesResult.architecture);
                Assert.Equal("Nexus 7", highEntropyValuesResult.model);
                Assert.Equal("Android", highEntropyValuesResult.platform);
                Assert.Equal("6.0.1", highEntropyValuesResult.platformVersion);
                Assert.Equal("89.0.4389.114", highEntropyValuesResult.uaFullVersion);
            }
        }
    }
}
