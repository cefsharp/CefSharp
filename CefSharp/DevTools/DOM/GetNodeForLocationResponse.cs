// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetNodeForLocationResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetNodeForLocationResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int backendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// backendNodeId
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
        /// frameId
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
        /// nodeId
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