// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ApplicationCache
{
    /// <summary>
    /// GetApplicationCacheForFrameResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetApplicationCacheForFrameResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.ApplicationCache.ApplicationCache applicationCache
        {
            get;
            set;
        }

        /// <summary>
        /// applicationCache
        /// </summary>
        public CefSharp.DevTools.ApplicationCache.ApplicationCache ApplicationCache
        {
            get
            {
                return applicationCache;
            }
        }
    }
}