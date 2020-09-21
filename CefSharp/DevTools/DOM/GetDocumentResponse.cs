// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetDocumentResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetDocumentResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.DOM.Node root
        {
            get;
            set;
        }

        /// <summary>
        /// root
        /// </summary>
        public CefSharp.DevTools.DOM.Node Root
        {
            get
            {
                return root;
            }
        }
    }
}