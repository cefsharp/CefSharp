// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetComputedStyleForNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetComputedStyleForNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSComputedStyleProperty> computedStyle
        {
            get;
            set;
        }

        /// <summary>
        /// computedStyle
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSComputedStyleProperty> ComputedStyle
        {
            get
            {
                return computedStyle;
            }
        }
    }
}