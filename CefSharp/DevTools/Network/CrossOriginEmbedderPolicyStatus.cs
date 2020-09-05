// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// CrossOriginEmbedderPolicyStatus
    /// </summary>
    public class CrossOriginEmbedderPolicyStatus : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }
    }
}