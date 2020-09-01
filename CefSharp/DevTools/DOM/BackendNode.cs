// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Backend node with a friendly name.
    /// </summary>
    public class BackendNode
    {
        /// <summary>
        /// `Node`'s nodeType.
        /// </summary>
        public int NodeType
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeName.
        /// </summary>
        public string NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int BackendNodeId
        {
            get;
            set;
        }
    }
}