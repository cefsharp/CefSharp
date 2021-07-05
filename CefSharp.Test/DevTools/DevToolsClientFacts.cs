// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        public async Task CanCaptureScreenshot()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadUrlAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var response = await devToolsClient.Page.CaptureScreenshotAsync();

                    Assert.NotNull(response.Data);
                    Assert.NotEmpty(response.Data);

                    var image = Image.FromStream(new MemoryStream(response.Data));
                    var size = browser.Size;

                    Assert.NotNull(image);
                    Assert.Equal(ImageFormat.Png, image.RawFormat);
                    Assert.Equal(size.Width, image.Width);
                    Assert.Equal(size.Height, image.Height);
                }
            }
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
                    var uab = new UserAgentBrandVersion
                    {
                        Brand = "Google Chrome",
                        Version = "89"
                    };
                    brandsList.Add(uab);

                    var uab2 = new UserAgentBrandVersion
                    {
                        Brand = "Chromium",
                        Version = "89"
                    };
                    brandsList.Add(uab2);

                    var ua = new UserAgentMetadata
                    {
                        Brands = brandsList,
                        Architecture = "arm",
                        Model = "Nexus 7",
                        Platform = "Android",
                        PlatformVersion = "6.0.1",
                        FullVersion = "89.0.4389.114",
                        Mobile = true
                    };

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

        [Fact]
        public async Task CanSetExtraHTTPHeaders()
        {
            using (var browser = new ChromiumWebBrowser("about:blank", automaticallyCreateBrowser: false))
            {
                await browser.CreateBrowserAsync();

                RequestWillBeSentEventArgs requestWillBeSentEventArgs = null;
                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var extraHeaders = new Headers();
                    extraHeaders.SetCommaSeparatedValues("TEST", "0");
                    extraHeaders.AppendCommaSeparatedValues("test", " 1 ", "\" 2 \"");
                    extraHeaders.AppendCommaSeparatedValues("Test", " 2,5 ");

                    await devToolsClient.Network.SetExtraHTTPHeadersAsync(extraHeaders);

                    devToolsClient.Network.RequestWillBeSent += (sender, args) =>
                    {
                        if (requestWillBeSentEventArgs == null)
                        {
                            requestWillBeSentEventArgs = args;
                        }
                    };

                    // enable events
                    await devToolsClient.Network.EnableAsync();

                    await browser.LoadUrlAsync("www.google.com");
                }

                Assert.NotNull(requestWillBeSentEventArgs);
                Assert.NotEmpty(requestWillBeSentEventArgs.RequestId);
                Assert.NotEqual(0, requestWillBeSentEventArgs.Timestamp);
                Assert.NotEqual(0, requestWillBeSentEventArgs.WallTime);
                Assert.NotNull(requestWillBeSentEventArgs.Request);
                Assert.True(requestWillBeSentEventArgs.Request.Headers.TryGetValues("TeSt", out var values));
                Assert.Collection(values,
                    v => Assert.Equal("0", v),
                    v => Assert.Equal("1", v),
                    v => Assert.Equal(" 2 ", v),
                    v => Assert.Equal(" 2,5 ", v)
                );
            }
        }

        [Fact]
        public async Task ExecuteDevToolsMethodThrowsExceptionWithInvalidMethod()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com"))
            {
                await browser.LoadUrlAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var ex = await Assert.ThrowsAsync<CefSharp.DevTools.DevToolsClientException>(
                        () => devToolsClient.ExecuteDevToolsMethodAsync("methoddoesnotexist"));

                    Assert.NotNull(ex.Response);
                    Assert.NotEqual(0, ex.Response.MessageId);
                    Assert.NotEqual(0, ex.Response.Code);
                    Assert.NotNull(ex.Response.Message);
                }
            }
        }
    }
}
