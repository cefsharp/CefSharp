// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Match data for a CSS rule.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RuleMatch : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// CSS rule in the match.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rule"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.CSSRule Rule
        {
            get;
            set;
        }

        /// <summary>
        /// Matching selector indices in the rule's selectorList selectors (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("matchingSelectors"), IsRequired = (true))]
        public int[] MatchingSelectors
        {
            get;
            set;
        }
    }
}