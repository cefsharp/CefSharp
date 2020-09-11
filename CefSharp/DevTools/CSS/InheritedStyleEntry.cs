// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Inherited CSS rule collection from ancestor node.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class InheritedStyleEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The ancestor node's inline style, if any, in the style inheritance chain.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inlineStyle"), IsRequired = (false))]
        public CefSharp.DevTools.CSS.CSSStyle InlineStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Matches of CSS rules matching the ancestor node in the style inheritance chain.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("matchedCSSRules"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleMatch> MatchedCSSRules
        {
            get;
            set;
        }
    }
}