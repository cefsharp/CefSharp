// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// type of HTTP response cached
    /// </summary>
    public enum CachedResponseType
    {
        /// <summary>
        /// basic
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("basic"))]
        Basic,
        /// <summary>
        /// cors
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cors"))]
        Cors,
        /// <summary>
        /// default
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("default"))]
        Default,
        /// <summary>
        /// error
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("error"))]
        Error,
        /// <summary>
        /// opaqueResponse
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("opaqueResponse"))]
        OpaqueResponse,
        /// <summary>
        /// opaqueRedirect
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("opaqueRedirect"))]
        OpaqueRedirect
    }
}