// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit.Abstractions;
using Xunit;

namespace CefSharp.Test.Framework
{
    public class ParseUrlTests
    {
        private readonly ITestOutputHelper output;

        public ParseUrlTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("https://google.com", "google.com")]
        public void CanParseGoogleDotComUrl(string url, string host)
        {
            var urlParts = Cef.ParseUrl(url);

            Assert.Equal(urlParts.Host, host);
        }

        [Theory]
        [InlineData("google.com")]
        public void CanParseInvalidUrl(string url)
        {
            var urlParts = Cef.ParseUrl(url);

            Assert.Null(urlParts);
        }
    }
}
