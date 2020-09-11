// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// StartPreciseCoverageResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StartPreciseCoverageResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// timestamp
        /// </summary>
        public long Timestamp
        {
            get
            {
                return timestamp;
            }
        }
    }
}