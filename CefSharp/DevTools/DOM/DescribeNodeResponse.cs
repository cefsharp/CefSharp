// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// DescribeNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DescribeNodeResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal Node node
        {
            get;
            set;
        }

        /// <summary>
        /// Node description.
        /// </summary>
        public Node Node
        {
            get
            {
                return node;
            }
        }
    }
}