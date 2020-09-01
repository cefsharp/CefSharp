// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket message data. This represents an entire WebSocket message, not just a fragmented frame as the name suggests.
    /// </summary>
    public class WebSocketFrame
    {
        /// <summary>
        /// WebSocket message opcode.
        /// </summary>
        public long Opcode
        {
            get;
            set;
        }

        /// <summary>
        /// WebSocket message mask.
        /// </summary>
        public bool Mask
        {
            get;
            set;
        }

        /// <summary>
        /// WebSocket message payload data.
        public string PayloadData
        {
            get;
            set;
        }
    }
}