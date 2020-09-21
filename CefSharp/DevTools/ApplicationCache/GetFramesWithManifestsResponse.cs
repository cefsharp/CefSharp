// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ApplicationCache
{
    /// <summary>
    /// GetFramesWithManifestsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetFramesWithManifestsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.ApplicationCache.FrameWithManifest> frameIds
        {
            get;
            set;
        }

        /// <summary>
        /// frameIds
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.ApplicationCache.FrameWithManifest> FrameIds
        {
            get
            {
                return frameIds;
            }
        }
    }
}