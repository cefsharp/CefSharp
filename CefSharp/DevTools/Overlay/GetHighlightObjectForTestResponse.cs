// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// GetHighlightObjectForTestResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetHighlightObjectForTestResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal object highlight
        {
            get;
            set;
        }

        /// <summary>
        /// highlight
        /// </summary>
        public object Highlight
        {
            get
            {
                return highlight;
            }
        }
    }
}