// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// DescribeNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DescribeNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.DOM.Node node
        {
            get;
            set;
        }

        /// <summary>
        /// node
        /// </summary>
        public CefSharp.DevTools.DOM.Node Node
        {
            get
            {
                return node;
            }
        }
    }
}