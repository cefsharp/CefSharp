// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// A cookie which was not stored from a response with the corresponding reason.
    /// </summary>
    public class BlockedSetCookieWithReason : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The reason(s) this cookie was blocked.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("blockedReasons"), IsRequired = (true))]
        public string BlockedReasons
        {
            get;
            set;
        }

        /// <summary>
        /// The string representing this individual cookie as it would appear in the header.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookieLine"), IsRequired = (true))]
        public string CookieLine
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie object which represents the cookie which was not stored. It is optional because
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookie"), IsRequired = (false))]
        public Cookie Cookie
        {
            get;
            set;
        }
    }
}