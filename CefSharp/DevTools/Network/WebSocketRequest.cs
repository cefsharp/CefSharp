// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket request data.
    /// </summary>
    public class WebSocketRequest
    {
        /// <summary>
        /// HTTP request headers.
        /// </summary>
        public Headers Headers
        {
            get;
            set;
        }
    }
}