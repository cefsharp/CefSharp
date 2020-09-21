// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// GetRealtimeDataResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetRealtimeDataResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.WebAudio.ContextRealtimeData realtimeData
        {
            get;
            set;
        }

        /// <summary>
        /// realtimeData
        /// </summary>
        public CefSharp.DevTools.WebAudio.ContextRealtimeData RealtimeData
        {
            get
            {
                return realtimeData;
            }
        }
    }
}