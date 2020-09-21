// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// RequestCachedResponseResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestCachedResponseResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CacheStorage.CachedResponse response
        {
            get;
            set;
        }

        /// <summary>
        /// response
        /// </summary>
        public CefSharp.DevTools.CacheStorage.CachedResponse Response
        {
            get
            {
                return response;
            }
        }
    }
}