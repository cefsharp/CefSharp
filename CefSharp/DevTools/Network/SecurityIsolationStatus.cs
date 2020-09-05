// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// SecurityIsolationStatus
    /// </summary>
    public class SecurityIsolationStatus : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("coop"), IsRequired = (true))]
        public CrossOriginOpenerPolicyStatus Coop
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("coep"), IsRequired = (true))]
        public CrossOriginEmbedderPolicyStatus Coep
        {
            get;
            set;
        }
    }
}