// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// SetStyleTextsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetStyleTextsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSStyle> styles
        {
            get;
            set;
        }

        /// <summary>
        /// styles
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSStyle> Styles
        {
            get
            {
                return styles;
            }
        }
    }
}