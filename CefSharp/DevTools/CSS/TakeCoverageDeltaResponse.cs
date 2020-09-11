// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// TakeCoverageDeltaResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class TakeCoverageDeltaResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleUsage> coverage
        {
            get;
            set;
        }

        /// <summary>
        /// coverage
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.RuleUsage> Coverage
        {
            get
            {
                return coverage;
            }
        }

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