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
        /// The `Accessibility.AXNode` for this DOM node, if it exists, plus its ancestors, siblings and
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXNode> Nodes
        {
            get
            {
                return nodes;
            }
        }
    }
}