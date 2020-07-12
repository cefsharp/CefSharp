// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Text;

namespace CefSharp.Web
{
    /// <summary>
    /// Represents an raw Html (not already encoded)
    /// When passed to a ChromiumWebBrowser constructor, the html will be converted to a Data Uri
    /// and loaded in the browser.
    /// See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs for details
    /// </summary>
    public class HtmlString
    {
        private readonly string html;
        private readonly bool base64Encode;

        /// <summary>
        /// Initializes a new instance of the HtmlString class.
        /// </summary>
        /// <param name="html">raw html string (not already encoded)</param>
        /// <param name="base64Encode">if true the html string will be base64 encoded using UTF8 encoding.</param>
        public HtmlString(string html, bool base64Encode = false)
        {
            this.base64Encode = base64Encode;
            this.html = html;
        }

        /// <summary>
        /// The html as a Data Uri encoded string
        /// </summary>
        /// <returns>data Uri string suitable for passing to <see cref="IWebBrowser.Load(string)"/></returns>
        public string ToDataUriString()
        {
            if (base64Encode)
            {
                var base64EncodedHtml = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
                return "data:text/html;base64," + base64EncodedHtml;
            }

            var uriEncodedHtml = Uri.EscapeDataString(html);
            return "data:text/html," + uriEncodedHtml;
        }

        /// <summary>
        /// HtmlString that will be base64 encoded
        /// </summary>
        /// <param name="html">raw html (not already encoded)</param>
        public static explicit operator HtmlString(string html)
        {
            return new HtmlString(html, true);
        }

        /// <summary>
        /// Creates a HtmlString for the given file name
        /// Uses <see cref="File.ReadAllText(string, Encoding)"/> to read the
        /// text using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>HtmlString</returns>
        public static HtmlString FromFile(string fileName)
        {
            var html = File.ReadAllText(fileName, Encoding.UTF8);

            return (HtmlString)html;
        }
    }
}
