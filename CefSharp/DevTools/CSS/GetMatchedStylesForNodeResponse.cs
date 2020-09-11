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
        /// Inline style for the specified DOM node.
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
        /// Attribute-defined element style (e.g. resulting from "width=20 height=100%").
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
        /// CSS rules matching this node, from all applicable stylesheets.
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
        /// Pseudo style matches for this node.
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
        /// A chain of inherited styles (from the immediate node parent up to the DOM tree root).
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
        /// A list of CSS keyframed animations matching this node.
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