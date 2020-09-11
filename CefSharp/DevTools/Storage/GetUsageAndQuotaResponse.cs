// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Storage
{
    /// <summary>
    /// GetUsageAndQuotaResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetUsageAndQuotaResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long usage
        {
            get;
            set;
        }

        /// <summary>
        /// usage
        /// </summary>
        public long Usage
        {
            get
            {
                return usage;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal long quota
        {
            get;
            set;
        }

        /// <summary>
        /// quota
        /// </summary>
        public long Quota
        {
            get
            {
                return quota;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Storage.UsageForType> usageBreakdown
        {
            get;
            set;
        }

        /// <summary>
        /// usageBreakdown
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Storage.UsageForType> UsageBreakdown
        {
            get
            {
                return usageBreakdown;
            }
        }
    }
}