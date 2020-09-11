// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// SafetyTipInfo
    /// </summary>
    public class SafetyTipInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Describes whether the page triggers any safety tips or reputation warnings. Default is unknown.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("safetyTipStatus"), IsRequired = (true))]
        public CefSharp.DevTools.Security.SafetyTipStatus SafetyTipStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The URL the safety tip suggested ("Did you mean?"). Only filled in for lookalike matches.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("safeUrl"), IsRequired = (false))]
        public string SafeUrl
        {
            get;
            set;
        }
    }
}