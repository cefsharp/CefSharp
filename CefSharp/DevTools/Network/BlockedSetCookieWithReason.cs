// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// A cookie which was not stored from a response with the corresponding reason.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BlockedSetCookieWithReason : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Network.SetCookieBlockedReason[] BlockedReasons
        {
            get
            {
                return (CefSharp.DevTools.Network.SetCookieBlockedReason[])(StringToEnum(typeof(CefSharp.DevTools.Network.SetCookieBlockedReason[]), blockedReasons));
            }

            set
            {
                blockedReasons = (EnumToString(value));
            }
        }

        /// <summary>
        /// The reason(s) this cookie was blocked.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("blockedReasons"), IsRequired = (true))]
        internal string blockedReasons
        {
            get;
            set;
        }

        /// <summary>
        /// The string representing this individual cookie as it would appear in the header.
        /// This is not the entire "cookie" or "set-cookie" header which could have multiple cookies.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookieLine"), IsRequired = (true))]
        public string CookieLine
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie object which represents the cookie which was not stored. It is optional because
        /// sometimes complete cookie information is not available, such as in the case of parsing
        /// errors.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookie"), IsRequired = (false))]
        public CefSharp.DevTools.Network.Cookie Cookie
        {
            get;
            set;
        }
    }
}