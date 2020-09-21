// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS coverage information.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RuleUsage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The css style sheet identifier (absent for user agent stylesheet and user-specified
        /// stylesheet rules) this rule came from.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (true))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// Offset of the start of the rule (including selector) from the beginning of the stylesheet.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startOffset"), IsRequired = (true))]
        public long StartOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Offset of the end of the rule body from the beginning of the stylesheet.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endOffset"), IsRequired = (true))]
        public long EndOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the rule was actually used by some element in the page.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("used"), IsRequired = (true))]
        public bool Used
        {
            get;
            set;
        }
    }
}