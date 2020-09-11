// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetResourceTreeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetResourceTreeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Page.FrameResourceTree frameTree
        {
            get;
            set;
        }

        /// <summary>
        /// frameTree
        /// </summary>
        public CefSharp.DevTools.Page.FrameResourceTree FrameTree
        {
            get
            {
                return frameTree;
            }
        }
    }
}