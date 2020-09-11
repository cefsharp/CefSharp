// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetMatchedStylesForNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetMatchedStylesForNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CSS.CSSStyle inlineStyle
        {
            get;
            set;
        }

        /// <summary>
        /// inlineStyle
        /// </summary>
        public CefSharp.DevTools.CSS.CSSStyle InlineStyle
        {
            get
            {
                return inlineStyle;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CSS.CSSStyle attributesStyle
        {
            get;
            set;
        }

        /// <summary>
        /// attributesStyle
        /// </summary>
        public CefSharp.DevTools.CSS.CSSStyle AttributesStyle
        {
            get
            {
                return attributesStyle;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleMatch> matchedCSSRules
        {
            get;
            set;
        }

        /// <summary>
        /// matchedCSSRules
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleMatch> MatchedCSSRules
        {
            get
            {
                return matchedCSSRules;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.PseudoElementMatches> pseudoElements
        {
            get;
            set;
        }

        /// <summary>
        /// pseudoElements
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.PseudoElementMatches> PseudoElements
        {
            get
            {
                return pseudoElements;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.InheritedStyleEntry> inherited
        {
            get;
            set;
        }

        /// <summary>
        /// inherited
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.InheritedStyleEntry> Inherited
        {
            get
            {
                return inherited;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSKeyframesRule> cssKeyframesRules
        {
            get;
            set;
        }

        /// <summary>
        /// cssKeyframesRules
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSKeyframesRule> CssKeyframesRules
        {
            get
            {
                return cssKeyframesRules;
            }
        }
    }
}