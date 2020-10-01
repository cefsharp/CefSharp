// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.Serialization;

namespace CefSharp.Internals.Wcf
{
    [DataContract]
    [KnownType(typeof(bool[]))]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(short[]))]
    [KnownType(typeof(int[]))]
    [KnownType(typeof(long[]))]
    [KnownType(typeof(ushort[]))]
    [KnownType(typeof(uint[]))]
    [KnownType(typeof(ulong[]))]
    [KnownType(typeof(float[]))]
    [KnownType(typeof(double[]))]
    [KnownType(typeof(string[]))]
    public class BrowserProcessResponse
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public object Result { get; set; }
    }
}
