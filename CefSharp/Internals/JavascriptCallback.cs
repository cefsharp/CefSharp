// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptCallback
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int BrowserId { get; set; }

        [DataMember]
        public string FrameId { get; set; }

        public byte[] ToByteArray(byte primitiveType)
        {
            var idBytes = BitConverter.GetBytes(Id);
            var browserIdBytes = BitConverter.GetBytes(BrowserId);
            var frameIdBytes = System.Text.Encoding.ASCII.GetBytes(FrameId);

            var bytes = new byte[1 + idBytes.Length + browserIdBytes.Length + frameIdBytes.Length];

            bytes[0] = primitiveType;
            idBytes.CopyTo(bytes, 1);
            browserIdBytes.CopyTo(bytes, 1 + idBytes.Length);
            frameIdBytes.CopyTo(bytes, 1 + idBytes.Length + browserIdBytes.Length);

            return bytes;
        }

        public static JavascriptCallback FromBytes(byte[] bytes)
        {
            var primativeType = bytes[0];
            var frameIdLength = bytes.Length - 1 - sizeof(long) - sizeof(int);
            var id = BitConverter.ToInt64(bytes, 1);
            var browserId = BitConverter.ToInt32(bytes, 1 + sizeof(long));
            var frameId = System.Text.Encoding.ASCII.GetString(bytes, 1 + sizeof(long) + sizeof(int), frameIdLength);

            var callback = new JavascriptCallback
            {
                Id = id,
                BrowserId = browserId,
                FrameId = frameId
            };

            return callback;
        }
    }
}
