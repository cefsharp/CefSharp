// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// GetRuntimeCallStatsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetRuntimeCallStatsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Profiler.CounterInfo> result
        {
            get;
            set;
        }

        /// <summary>
        /// result
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Profiler.CounterInfo> Result
        {
            get
            {
                return result;
            }
        }
    }
}