// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// MixedContentIssueDetails
    /// </summary>
    public class MixedContentIssueDetails : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The type of resource causing the mixed content issue (css, js, iframe,
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("resourceType"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.MixedContentResourceType? ResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// The way the mixed content issue is being resolved.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("resolutionStatus"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.MixedContentResolutionStatus ResolutionStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The unsafe http url causing the mixed content issue.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("insecureURL"), IsRequired = (true))]
        public string InsecureURL
        {
            get;
            set;
        }

        /// <summary>
        /// The url responsible for the call to an unsafe url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mainResourceURL"), IsRequired = (true))]
        public string MainResourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// The mixed content request.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("request"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.AffectedRequest Request
        {
            get;
            set;
        }

        /// <summary>
        /// Optional because not every mixed content issue is necessarily linked to a frame.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frame"), IsRequired = (false))]
        public CefSharp.DevTools.Audits.AffectedFrame Frame
        {
            get;
            set;
        }
    }
}