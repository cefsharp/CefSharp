// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// GetPartialAXTreeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPartialAXTreeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXNode> nodes
        {
            get;
            set;
        }

        /// <summary>
        /// nodes
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXNode> Nodes
        {
            get
            {
                return nodes;
            }
        }
    }
}