// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// GetHeapUsageResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetHeapUsageResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long usedSize
        {
            get;
            set;
        }

        /// <summary>
        /// usedSize
        /// </summary>
        public long UsedSize
        {
            get
            {
                return usedSize;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal long totalSize
        {
            get;
            set;
        }

        /// <summary>
        /// totalSize
        /// </summary>
        public long TotalSize
        {
            get
            {
                return totalSize;
            }
        }
    }
}