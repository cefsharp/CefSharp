// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// RequestEntriesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestEntriesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.DataEntry> cacheDataEntries
        {
            get;
            set;
        }

        /// <summary>
        /// Array of object store data entries.
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.DataEntry> CacheDataEntries
        {
            get
            {
                return cacheDataEntries;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal long returnCount
        {
            get;
            set;
        }

        /// <summary>
        /// Count of returned entries from this storage. If pathFilter is empty, it
        public long ReturnCount
        {
            get
            {
                return returnCount;
            }
        }
    }
}