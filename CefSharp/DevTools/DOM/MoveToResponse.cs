// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// MoveToResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class MoveToResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int nodeId
        {
            get;
            set;
        }

        /// <summary>
        /// New id of the moved node.
        /// </summary>
        public int NodeId
        {
            get
            {
                return nodeId;
            }
        }
    }
}