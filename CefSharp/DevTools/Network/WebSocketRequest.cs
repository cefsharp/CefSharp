// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket request data.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class WebSocketRequest : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// HTTP request headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headers"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Headers Headers
        {
            get;
            set;
        }
    }
}