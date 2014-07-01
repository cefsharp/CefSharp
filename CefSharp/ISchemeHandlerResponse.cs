// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;
using System.IO;

namespace CefSharp
{
    public interface ISchemeHandlerResponse
    {
        /// <summary>
        /// A Stream with the response data. If the request didn't return any response, leave this property as null.
        /// </summary>
        Stream ResponseStream { get; set; }

        string MimeType { get; set; }

        NameValueCollection ResponseHeaders { get; set; }

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// The length of the response contents. Defaults to -1, which means unknown length
        /// and causes CefSharp to read the response stream in pieces. Thus, setting a length
        /// is optional but allows for more optimal response reading.
        /// </summary>
        int ContentLength { get; set; }

        /// <summary>
        /// URL to redirect to (leave empty to not redirect).
        /// </summary>
        string RedirectUrl { get; set; }

        /// <summary>
        /// Set to true to close the response stream once it has been read. The default value
        /// is false in order to preserve the old CefSharp behavior of not closing the stream.
        /// </summary>
        bool CloseStream { get; set; }
    }
}
