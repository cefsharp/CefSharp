// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// WebSocket message data. This represents an entire WebSocket message, not just a fragmented frame as the name suggests.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class WebSocketFrame : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// WebSocket message opcode.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("opcode"), IsRequired = (true))]
        public long Opcode
        {
            get;
            set;
        }

        /// <summary>
        /// WebSocket message mask.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mask"), IsRequired = (true))]
        public bool Mask
        {
            get;
            set;
        }

        /// <summary>
        /// WebSocket message payload data.
        /// If the opcode is 1, this is a text message and payloadData is a UTF-8 string.
        /// If the opcode isn't 1, then payloadData is a base64 encoded string representing binary data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("payloadData"), IsRequired = (true))]
        public string PayloadData
        {
            get;
            set;
        }
    }
}