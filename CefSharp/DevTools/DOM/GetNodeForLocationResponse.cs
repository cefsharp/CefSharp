// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetNodeForLocationResponse
    /// </summary>
    public class GetNodeForLocationResponse
    {
        /// <summary>
        /// Resulting node.
        /// </summary>
        public int backendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame this node belongs to.
        /// </summary>
        public string frameId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the node at given coordinates, only when enabled and requested document.
        /// </summary>
        public int? nodeId
        {
            get;
            set;
        }
    }
}