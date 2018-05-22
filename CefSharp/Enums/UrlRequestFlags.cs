// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
using System;

namespace CefSharp
{
    /// <summary>
    /// Flags used to customize the behavior of CefURLRequest.
    /// </summary>
    [Flags]
    public enum UrlRequestFlags : int
    {
        /// <summary>
        /// Default behavior.
        /// </summary>
        None = 0,

        /// <summary>
        /// If set the cache will be skipped when handling the request.
        /// </summary>
        SkipCache = 1 << 0,

        /// <summary>
        /// If set user name, password, and cookies may be sent with the request, and
        /// cookies may be saved from the response.
        /// </summary>
        AllowCachedCredentials = 1 << 1,

        /// <summary>
        ///  If set upload progress events will be generated when a request has a body.
        /// </summary>
        ReportUploadProgress = 1 << 3,

        /// <summary>
        /// If set the CefURLRequestClient::OnDownloadData method will not be called.
        /// </summary>
        NoDownloadData = 1 << 6,

        /// <summary>
        /// If set 5XX redirect errors will be propagated to the observer instead of
        /// automatically re-tried. This currently only applies for requests
        /// originated in the browser process.
        /// </summary>
        NoRetryOn5XX = 1 << 7,
    }
}
