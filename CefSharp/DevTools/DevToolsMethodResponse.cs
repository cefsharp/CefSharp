// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Method Response
    /// </summary>
    public class DevToolsMethodResponse
    {
        /// <summary>
        /// MessageId
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Method Response as Json string
        /// </summary>
        public string ResponseAsJsonString { get; set; }

        internal T DeserializeJson<T>()
        {
            if (Success)
            {
                var bytes = Encoding.UTF8.GetBytes(ResponseAsJsonString);
                using (var ms = new MemoryStream(bytes))
                {
                    var dcs = new DataContractJsonSerializer(typeof(T));
                    return (T)dcs.ReadObject(ms);
                }
            }

            throw new DevToolsClientException(ResponseAsJsonString);
        }

    }
}
