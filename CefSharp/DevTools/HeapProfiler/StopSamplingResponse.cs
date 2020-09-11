// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// StopSamplingResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StopSamplingResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.HeapProfiler.SamplingHeapProfile profile
        {
            get;
            set;
        }

        /// <summary>
        /// profile
        /// </summary>
        public CefSharp.DevTools.HeapProfiler.SamplingHeapProfile Profile
        {
            get
            {
                return profile;
            }
        }
    }
}