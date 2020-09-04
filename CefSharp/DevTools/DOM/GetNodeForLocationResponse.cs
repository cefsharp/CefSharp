// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetNodeForLocationResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetNodeForLocationResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int backendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Resulting node.
        /// </summary>
        public int BackendNodeId
        {
            get
            {
                return backendNodeId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string frameId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame this node belongs to.
        /// </summary>
        public string FrameId
        {
            get
            {
                return frameId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int? nodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the node at given coordinates, only when enabled and requested document.
        /// </summary>
        public int? NodeId
        {
            get
            {
                return nodeId;
            }
        }
    }
}