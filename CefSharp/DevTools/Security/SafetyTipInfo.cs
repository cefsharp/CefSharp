// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// SafetyTipInfo
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SafetyTipInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Security.SafetyTipStatus SafetyTipStatus
        {
            get
            {
                return (CefSharp.DevTools.Security.SafetyTipStatus)(StringToEnum(typeof(CefSharp.DevTools.Security.SafetyTipStatus), safetyTipStatus));
            }

            set
            {
                safetyTipStatus = (EnumToString(value));
            }
        }

        /// <summary>
        /// Describes whether the page triggers any safety tips or reputation warnings. Default is unknown.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("safetyTipStatus"), IsRequired = (true))]
        internal string safetyTipStatus
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