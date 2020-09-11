// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// RequestDatabaseResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestDatabaseResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.IndexedDB.DatabaseWithObjectStores databaseWithObjectStores
        {
            get;
            set;
        }

        /// <summary>
        /// databaseWithObjectStores
        /// </summary>
        public CefSharp.DevTools.IndexedDB.DatabaseWithObjectStores DatabaseWithObjectStores
        {
            get
            {
                return databaseWithObjectStores;
            }
        }
    }
}