// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// RequestDatabaseNamesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestDatabaseNamesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] databaseNames
        {
            get;
            set;
        }

        /// <summary>
        /// databaseNames
        /// </summary>
        public string[] DatabaseNames
        {
            get
            {
                return databaseNames;
            }
        }
    }
}