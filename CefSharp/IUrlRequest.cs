// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Class used to make a URL request. URL requests are not associated with
    /// a browser instance so no CefClient callbacks will be executed.
    /// URL requests can be created on any valid CEF thread in either the browser
    /// or render process. Once created the methods of the URL request object must
    /// be accessed on the same thread that created it. 
    /// </summary>
    public interface IUrlRequest : IDisposable
    {
        /// <summary>
        /// True if the response was served from the cache.
        /// </summary>
        bool ResponseWasCached { get; }

        /// <summary>
        /// The response, or null if no response information is available
        /// </summary>
        IResponse Response { get; }

        /// <summary>
        /// The request status.
        /// </summary>
        UrlRequestStatus RequestStatus { get; }
    }
}
