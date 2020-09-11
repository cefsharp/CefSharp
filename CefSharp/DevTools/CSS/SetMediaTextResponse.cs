// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// SetMediaTextResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetMediaTextResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CSS.CSSMedia media
        {
            get;
            set;
        }

        /// <summary>
        /// media
        /// </summary>
        public CefSharp.DevTools.CSS.CSSMedia Media
        {
            get
            {
                return media;
            }
        }
    }
}