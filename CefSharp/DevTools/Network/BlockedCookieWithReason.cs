// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// A cookie with was not sent with a request with the corresponding reason.
    /// </summary>
    public class BlockedCookieWithReason : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The reason(s) the cookie was blocked.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("blockedReasons"), IsRequired = (true))]
        public CefSharp.DevTools.Network.CookieBlockedReason[] BlockedReasons
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie object representing the cookie which was not sent.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookie"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Cookie Cookie
        {
            get;
            set;
        }
    }
}