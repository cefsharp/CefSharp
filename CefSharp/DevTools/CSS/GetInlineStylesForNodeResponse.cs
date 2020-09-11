// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetInlineStylesForNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetInlineStylesForNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
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
    }
}