// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// This struct holds a list of optional fields with additional information
    public class InspectorIssueDetails : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// SameSiteCookieIssueDetails
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sameSiteCookieIssueDetails"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.SameSiteCookieIssueDetails SameSiteCookieIssueDetails
        {
            get;
            set;
        }

        /// <summary>
        /// MixedContentIssueDetails
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mixedContentIssueDetails"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.MixedContentIssueDetails MixedContentIssueDetails
        {
            get;
            set;
        }
    }
}