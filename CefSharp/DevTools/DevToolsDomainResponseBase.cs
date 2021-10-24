// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.DevTools
{
    [System.Runtime.Serialization.DataContractAttribute]
    public abstract class DevToolsDomainResponseBase
    {
        public byte[] Convert(string data)
        {
            return System.Convert.FromBase64String(data);
        }
    }
}
