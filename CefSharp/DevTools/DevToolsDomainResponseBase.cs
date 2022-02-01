// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevToolsDomainResponseBase
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public abstract class DevToolsDomainResponseBase
    {
        /// <summary>
        /// Convert from string to base64 byte array
        /// </summary>
        /// <param name="data">string data</param>
        /// <returns>byte array</returns>
        public byte[] Convert(string data)
        {
            return System.Convert.FromBase64String(data);
        }
    }
}
