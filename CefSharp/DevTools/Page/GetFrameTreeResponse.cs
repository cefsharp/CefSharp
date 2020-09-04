// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetFrameTreeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetFrameTreeResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal FrameTree frameTree
        {
            get;
            set;
        }

        /// <summary>
        /// Present frame tree structure.
        /// </summary>
        public FrameTree FrameTree
        {
            get
            {
                return frameTree;
            }
        }
    }
}