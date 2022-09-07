// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit;
using System;
using CefSharp.Example;
using System.Linq;

namespace CefSharp.Test.CookieManager
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class CookieManagerFacts : IClassFixture<ChromiumWebBrowserOffScreenFixture>
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;
        private readonly ChromiumWebBrowserOffScreenFixture classFixture;

        public CookieManagerFacts(ITestOutputHelper output, CefSharpFixture collectionFixture, ChromiumWebBrowserOffScreenFixture classFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
            this.classFixture = classFixture;
        }

        [Fact]
        //https://github.com/cefsharp/CefSharp/issues/4234
        public async Task CanSetAndGetCookie()
        {
            const string CookieName = "CefSharpExpiryTestCookie";
            var testStartDate = DateTime.Now;
            var expectedExpiry = DateTime.Now.AddDays(1);

            var browser = classFixture.Browser;

            Assert.False(browser.IsLoading);

            var cookieManager = browser.GetCookieManager();

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

            output.WriteLine("Expected {0} : Actual {1}", expectedExpiry, cookie.Expires.Value);
        }
    }
}
