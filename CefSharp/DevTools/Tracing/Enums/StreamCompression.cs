// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Tracing
{
    /// <summary>
    /// Compression type to use for traces returned via streams.
    /// </summary>
    public enum StreamCompression
    {
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None,
        /// <summary>
        /// gzip
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("gzip"))]
        Gzip
    }
}