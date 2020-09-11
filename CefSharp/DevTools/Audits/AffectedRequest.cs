// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// Information about a request that is affected by an inspector issue.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AffectedRequest : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The unique request id.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestId"), IsRequired = (true))]
        public string RequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Url
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }
    }
}