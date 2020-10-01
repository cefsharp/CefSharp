// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// StopResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StopResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Profiler.Profile profile
        {
            get;
            set;
        }

        /// <summary>
        /// profile
        /// </summary>
        public CefSharp.DevTools.Profiler.Profile Profile
        {
            get
            {
                return profile;
            }
        }
    }
}