// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket response data.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class WebSocketResponse : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// HTTP response status code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("status"), IsRequired = (true))]
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("statusText"), IsRequired = (true))]
        public string StatusText
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headers"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Headers Headers
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headersText"), IsRequired = (false))]
        public string HeadersText
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestHeaders"), IsRequired = (false))]
        public CefSharp.DevTools.Network.Headers RequestHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestHeadersText"), IsRequired = (false))]
        public string RequestHeadersText
        {
            get;
            set;
        }
    }
}