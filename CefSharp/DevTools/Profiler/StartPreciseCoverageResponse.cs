// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// StartPreciseCoverageResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StartPreciseCoverageResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Monotonically increasing time (in seconds) when the coverage update was taken in the backend.
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