// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// This information is currently necessary, as the front-end has a difficult
    public class SameSiteCookieIssueDetails : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Cookie
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookie"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.AffectedCookie Cookie
        {
            get;
            set;
        }

        /// <summary>
        /// CookieWarningReasons
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookieWarningReasons"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.SameSiteCookieWarningReason[] CookieWarningReasons
        {
            get;
            set;
        }

        /// <summary>
        /// CookieExclusionReasons
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookieExclusionReasons"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.SameSiteCookieExclusionReason[] CookieExclusionReasons
        {
            get;
            set;
        }

        /// <summary>
        /// Optionally identifies the site-for-cookies and the cookie url, which
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("operation"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.SameSiteCookieOperation Operation
        {
            get;
            set;
        }

        /// <summary>
        /// SiteForCookies
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("siteForCookies"), IsRequired = (false))]
        public string SiteForCookies
        {
            get;
            set;
        }

        /// <summary>
        /// CookieUrl
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cookieUrl"), IsRequired = (false))]
        public string CookieUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Request
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("request"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.AffectedRequest Request
        {
            get;
            set;
        }
    }
}