// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// MimeTypeMappingFacts - Tests file extension to mimeType mapping 
    /// </summary>
    public class MimeTypeMappingFacts
    {
        [Theory]
        [InlineData("html", "text/html")]
        [InlineData(".wasm", "application/wasm")]
        [InlineData(".ogg", "audio/ogg")]
        [InlineData(".oga", "audio/ogg")]
        [InlineData(".ogv", "video/ogg")]
        [InlineData(".opus", "audio/ogg")]
        [InlineData(".webm", "video/webm")]
        [InlineData(".weba", "audio/webm")]
        [InlineData(".webp", "image/webp")]
        [InlineData(".epub", "application/epub+zip")]
        [InlineData(".woff", "application/font-woff")]
        [InlineData(".woff2", "font/woff2")]
        [InlineData(".ttf", "font/ttf")]
        [InlineData(".otf", "font/otf")]
        [InlineData(".dummyextension", "application/octet-stream")]
        public void MapFileExtensionToMimeTypeTheory(string fileExtension, string expectedMimeType)
        {
            var actualMimeType = Cef.GetMimeType(fileExtension);
            Assert.Equal(expectedMimeType, actualMimeType);
        }
    }
}
