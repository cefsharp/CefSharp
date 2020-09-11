// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// GetObjectByHeapObjectIdResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetObjectByHeapObjectIdResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.RemoteObject result
        {
            get;
            set;
        }

        /// <summary>
        /// result
        /// </summary>
        public CefSharp.DevTools.Runtime.RemoteObject Result
        {
            get
            {
                return result;
            }
        }
    }
}