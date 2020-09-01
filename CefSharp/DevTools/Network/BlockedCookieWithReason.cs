// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// A cookie with was not sent with a request with the corresponding reason.
    /// </summary>
    public class BlockedCookieWithReason
    {
        /// <summary>
        /// The reason(s) the cookie was blocked.
        /// </summary>
        public string BlockedReasons
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie object representing the cookie which was not sent.
        /// </summary>
        public Cookie Cookie
        {
            get;
            set;
        }
    }
}