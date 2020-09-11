// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Tracing
{
    /// <summary>
    /// Data format of a trace. Can be either the legacy JSON format or the
    /// protocol buffer format. Note that the JSON format will be deprecated soon.
    /// </summary>
    public enum StreamFormat
    {
        /// <summary>
        /// json
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("json"))]
        Json,
        /// <summary>
        /// proto
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("proto"))]
        Proto
    }
}