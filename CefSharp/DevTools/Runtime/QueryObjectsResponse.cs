// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// QueryObjectsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class QueryObjectsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.RemoteObject objects
        {
            get;
            set;
        }

        /// <summary>
        /// objects
        /// </summary>
        public CefSharp.DevTools.Runtime.RemoteObject Objects
        {
            get
            {
                return objects;
            }
        }
    }
}