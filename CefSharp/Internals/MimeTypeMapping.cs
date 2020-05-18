// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Internal Mime Type Mappings.
    /// </summary>
    public static class MimeTypeMapping
    {
        /// <summary>
        /// Dictionary containing our custom mimeType mapping, you can add your own file extension
        /// to mimeType mappings to this dictionary.
        /// </summary>
        public static readonly IDictionary<string, string> CustomMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            // Recently added entries from 
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types
            // https://cs.chromium.org/chromium/src/net/base/mime_util.cc?sq=package:chromium&g=0&l=147
            // https://www.w3.org/TR/WOFF2/#IMT
            // https://tools.ietf.org/html/rfc8081#section-4.4.6
            {"woff2", "font/woff2"},
            {"ttf", "font/ttf"},
            {"otf", "font/otf"}
        };

        /// <summary>
        /// Lookup MimeType from the <see cref="CustomMappings"/>
        /// dictionary based on file extension.
        /// </summary>
        /// <param name="extension">extension</param>
        /// <returns>custom mimeType or application/octet-stream if no mapping found </returns>
        public static string GetCustomMapping(string extension)
        {
            string mime;
            return CustomMappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
    }
}
