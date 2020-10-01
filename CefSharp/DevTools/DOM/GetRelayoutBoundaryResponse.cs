// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetRelayoutBoundaryResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetRelayoutBoundaryResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int nodeId
        {
            get;
            set;
        }

        /// <summary>
        /// nodeId
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