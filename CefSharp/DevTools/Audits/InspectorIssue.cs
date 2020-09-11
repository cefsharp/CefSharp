// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// An inspector issue reported from the back-end.
    /// </summary>
    public class InspectorIssue : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Code
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("code"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.InspectorIssueCode Code
        {
            get;
            set;
        }

        /// <summary>
        /// Details
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("details"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.InspectorIssueDetails Details
        {
            get;
            set;
        }
    }
}