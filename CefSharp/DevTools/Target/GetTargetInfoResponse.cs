// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// GetTargetInfoResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetTargetInfoResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Target.TargetInfo targetInfo
        {
            get;
            set;
        }

        /// <summary>
        /// targetInfo
        /// </summary>
        public CefSharp.DevTools.Target.TargetInfo TargetInfo
        {
            get
            {
                return targetInfo;
            }
        }
    }
}