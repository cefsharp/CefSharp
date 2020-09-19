// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// This struct holds a list of optional fields with additional information
    /// specific to the kind of issue. When adding a new issue code, please also
    /// add a new optional field to this type.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
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

        /// <summary>
        /// BlockedByResponseIssueDetails
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("blockedByResponseIssueDetails"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.BlockedByResponseIssueDetails BlockedByResponseIssueDetails
        {
            get;
            set;
        }

        /// <summary>
        /// HeavyAdIssueDetails
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("heavyAdIssueDetails"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.HeavyAdIssueDetails HeavyAdIssueDetails
        {
            get;
            set;
        }
    }
}