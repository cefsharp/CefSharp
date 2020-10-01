// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// RequestDataResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestDataResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.IndexedDB.DataEntry> objectStoreDataEntries
        {
            get;
            set;
        }

        /// <summary>
        /// objectStoreDataEntries
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.IndexedDB.DataEntry> ObjectStoreDataEntries
        {
            get
            {
                return objectStoreDataEntries;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool hasMore
        {
            get;
            set;
        }

        /// <summary>
        /// hasMore
        /// </summary>
        public bool HasMore
        {
            get
            {
                return hasMore;
            }
        }
    }
}