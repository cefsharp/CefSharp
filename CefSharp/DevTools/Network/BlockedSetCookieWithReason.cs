// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// A cookie which was not stored from a response with the corresponding reason.
    /// </summary>
    public class BlockedSetCookieWithReason
    {
        /// <summary>
        /// The reason(s) this cookie was blocked.
        /// </summary>
        public string BlockedReasons
        {
            get;
            set;
        }

        /// <summary>
        /// The string representing this individual cookie as it would appear in the header.
        public string CookieLine
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie object which represents the cookie which was not stored. It is optional because
        public Cookie Cookie
        {
            get;
            set;
        }
    }
}