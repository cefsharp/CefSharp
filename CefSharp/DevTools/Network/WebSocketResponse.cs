// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket response data.
    /// </summary>
    public class WebSocketResponse
    {
        /// <summary>
        /// HTTP response status code.
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status text.
        /// </summary>
        public string StatusText
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers.
        /// </summary>
        public Headers Headers
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers text.
        /// </summary>
        public string HeadersText
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers.
        /// </summary>
        public Headers RequestHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers text.
        /// </summary>
        public string RequestHeadersText
        {
            get;
            set;
        }
    }
}