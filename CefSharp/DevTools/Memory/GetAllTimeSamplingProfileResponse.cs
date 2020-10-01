// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    /// <summary>
    /// GetAllTimeSamplingProfileResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetAllTimeSamplingProfileResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Memory.SamplingProfile profile
        {
            get;
            set;
        }

        /// <summary>
        /// profile
        /// </summary>
        public CefSharp.DevTools.Memory.SamplingProfile Profile
        {
            get
            {
                return profile;
            }
        }
    }
}