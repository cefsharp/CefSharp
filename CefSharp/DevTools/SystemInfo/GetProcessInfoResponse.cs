// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// GetProcessInfoResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetProcessInfoResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.ProcessInfo> processInfo
        {
            get;
            set;
        }

        /// <summary>
        /// processInfo
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.ProcessInfo> ProcessInfo
        {
            get
            {
                return processInfo;
            }
        }
    }
}