// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// RequestCacheNamesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestCacheNamesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Cache> caches
        {
            get;
            set;
        }

        /// <summary>
        /// caches
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Cache> Caches
        {
            get
            {
                return caches;
            }
        }
    }
}