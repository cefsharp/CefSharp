// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetFrameTreeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetFrameTreeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Page.FrameTree frameTree
        {
            get;
            set;
        }

        /// <summary>
        /// frameTree
        /// </summary>
        public CefSharp.DevTools.Page.FrameTree FrameTree
        {
            get
            {
                return frameTree;
            }
        }
    }
}