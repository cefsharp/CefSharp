// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit;
using System;
using CefSharp.Example;
using System.Linq;
using System.Collections.Generic;
using CefSharp.Enums;

namespace CefSharp.Test.CookieManager
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class CookieManagerTests : RequestContextIsolatedBrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;

        public CookieManagerTests(ITestOutputHelper output, CefSharpFixture collectionFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
        }

        [Fact]
        public async Task ShouldWork()
        {
            AssertInitialLoadComplete();

            const string expected = "password=123456";

            var cookieManager = Browser.GetCookieManager();

            var success = await cookieManager.SetCookieAsync(CefExample.HelloWorldUrl, new Cookie
            {
                Name = "password",
                Value = "123456"
            });

            Assert.True(success);

            var actual = await Browser.EvaluateScriptAsync<string>("document.cookie");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ShouldSetMultipleCookies()
        {
            AssertInitialLoadComplete();

            var expected = new string[]
                {
                    "multiple-1=123456",
                    "multiple-2=bar"
                };

            var cookieManager = Browser.GetCookieManager();

            var success = await cookieManager.SetCookieAsync(CefExample.HelloWorldUrl, new Cookie
            {
                Name = "multiple-1",
                Value = "123456"
            });

            success &= await cookieManager.SetCookieAsync(CefExample.HelloWorldUrl, new Cookie
            {
                Name = "multiple-2",
                Value = "bar"
            });

            Assert.True(success);

            var actual = await Browser.EvaluateScriptAsync<List<object>>(@"(() => {
                    const cookies = document.cookie.split(';');
                    return cookies.map(cookie => cookie.trim()).sort();
                })();");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ShouldGetACookie()
        {
            AssertInitialLoadComplete();

            var response = await Browser.EvaluateScriptAsync<string>(@"(() => {
                document.cookie = 'username=John Doe';
                return document.cookie;
            })();");

            Assert.Equal("username=John Doe", response);

            var cookieManager = Browser.GetCookieManager();

            var cookie = (await cookieManager.VisitAllCookiesAsync()).Single();

            Assert.Equal("username", cookie.Name);
            Assert.Equal("John Doe", cookie.Value);
            Assert.Equal("cefsharp.example", cookie.Domain);
            Assert.Equal("/", cookie.Path);
            Assert.Null(cookie.Expires);
            Assert.False(cookie.HttpOnly);
            Assert.False(cookie.Secure);
            Assert.Equal(CookieSameSite.Unspecified, cookie.SameSite);
        }

        [Fact]
        public async Task ShouldProperlyReportSecureCookie()
        {
            AssertInitialLoadComplete();

            var response = await Browser.EvaluateScriptAsync<string>(@"(() => {
                document.cookie = 'username=John Doe;Secure;';
                return document.cookie;
            })();");

            Assert.Equal("username=John Doe", response);

            var cookieManager = Browser.GetCookieManager();

            var cookie = (await cookieManager.VisitAllCookiesAsync()).Single();

            Assert.True(cookie.Secure);
        }

        [Fact]
        public async Task ShouldProperlyReportStrictSameSiteCookie()
        {
            AssertInitialLoadComplete();

            var response = await Browser.EvaluateScriptAsync<string>(@"(() => {
                document.cookie = 'username=John Doe;SameSite=Strict;';
                return document.cookie;
            })();");

            Assert.Equal("username=John Doe", response);

            var cookieManager = Browser.GetCookieManager();

            var cookie = (await cookieManager.VisitAllCookiesAsync()).Single();

            Assert.Equal(CookieSameSite.StrictMode, cookie.SameSite);
        }

        [Fact]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task ShouldSetAndGetCookie()
        {
            AssertInitialLoadComplete();

            const string CookieName = "CefSharpExpiryTestCookie";
            var testStartDate = DateTime.Now;
            var expectedExpiry = DateTime.Now.AddDays(1);

            var cookieManager = Browser.GetCookieManager();
            await cookieManager.DeleteCookiesAsync(CefExample.HelloWorldUrl, CookieName);

            var cookieSet = await cookieManager.SetCookieAsync(CefExample.HelloWorldUrl, new Cookie
            {
                Name = CookieName,
                Value = "ILikeCookies",
                Expires = expectedExpiry
            });

            Assert.True(cookieSet);

            var cookies = await cookieManager.VisitUrlCookiesAsync(CefExample.HelloWorldUrl, false);
            var cookie = cookies.First(x => x.Name == CookieName);

            Assert.True(cookie.Expires.HasValue);
            // Little bit of a loss in precision
            Assert.Equal(expectedExpiry, cookie.Expires.Value, TimeSpan.FromMilliseconds(10));
            Assert.Equal(cookie.Creation ,testStartDate, TimeSpan.FromMilliseconds(1000));
            Assert.Equal(cookie.LastAccess, testStartDate, TimeSpan.FromMilliseconds(1000));

            output.WriteLine("Expected {0} : Actual {1}", expectedExpiry, cookie.Expires.Value);
        }

        [Fact]
        public async Task ShouldClearCookies()
        {
            AssertInitialLoadComplete();

            var cookieManager = Browser.GetCookieManager();

            var cookieSet = await cookieManager.SetCookieAsync(CefExample.HelloWorldUrl, new Cookie
            {
                Name = "cookie1",
                Value = "1"
            });

            Assert.True(cookieSet);

            var response = await Browser.EvaluateScriptAsync<string>("document.cookie");

            Assert.Equal("cookie1=1", response);

            await cookieManager.DeleteCookiesAsync();

            var cookies = await cookieManager.VisitAllCookiesAsync();

            Assert.Empty(cookies);

            Browser.Reload();

            await Browser.WaitForNavigationAsync();

            response = await Browser.EvaluateScriptAsync<string>("document.cookie");

            Assert.Equal(string.Empty, response);
        }
    }
}
