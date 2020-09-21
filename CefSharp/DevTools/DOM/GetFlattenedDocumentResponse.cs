// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetFlattenedDocumentResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetFlattenedDocumentResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.DOM.Node> nodes
        {
            get;
            set;
        }

        /// <summary>
        /// nodes
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.DOM.Node> Nodes
        {
            get
            {
                return nodes;
            }
        }
    }
}