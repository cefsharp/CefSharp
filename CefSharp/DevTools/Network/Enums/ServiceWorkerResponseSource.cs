// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Source of serviceworker response.
    /// </summary>
    public enum ServiceWorkerResponseSource
    {
        /// <summary>
        /// cache-storage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cache-storage"))]
        CacheStorage,
        /// <summary>
        /// http-cache
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("http-cache"))]
        HttpCache,
        /// <summary>
        /// fallback-code
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("fallback-code"))]
        FallbackCode,
        /// <summary>
        /// network
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("network"))]
        Network
    }
}