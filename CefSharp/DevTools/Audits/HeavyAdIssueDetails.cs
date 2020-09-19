// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// HeavyAdIssueDetails
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class HeavyAdIssueDetails : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Audits.HeavyAdResolutionStatus Resolution
        {
            get
            {
                return (CefSharp.DevTools.Audits.HeavyAdResolutionStatus)(StringToEnum(typeof(CefSharp.DevTools.Audits.HeavyAdResolutionStatus), resolution));
            }

            set
            {
                resolution = (EnumToString(value));
            }
        }

        /// <summary>
        /// The resolution status, either blocking the content or warning.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("resolution"), IsRequired = (true))]
        internal string resolution
        {
            get;
            set;
        }

        public CefSharp.DevTools.Audits.HeavyAdReason Reason
        {
            get
            {
                return (CefSharp.DevTools.Audits.HeavyAdReason)(StringToEnum(typeof(CefSharp.DevTools.Audits.HeavyAdReason), reason));
            }

            set
            {
                reason = (EnumToString(value));
            }
        }

        /// <summary>
        /// The reason the ad was blocked, total network or cpu or peak cpu.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("reason"), IsRequired = (true))]
        internal string reason
        {
            get;
            set;
        }

        /// <summary>
        /// The frame that was blocked.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frame"), IsRequired = (true))]
        public CefSharp.DevTools.Audits.AffectedFrame Frame
        {
            get;
            set;
        }
    }
}