// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Storage
{
    /// <summary>
    /// Enum of possible storage types.
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// appcache
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("appcache"))]
        Appcache,
        /// <summary>
        /// cookies
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cookies"))]
        Cookies,
        /// <summary>
        /// file_systems
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("file_systems"))]
        File_systems,
        /// <summary>
        /// indexeddb
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("indexeddb"))]
        Indexeddb,
        /// <summary>
        /// local_storage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("local_storage"))]
        Local_storage,
        /// <summary>
        /// shader_cache
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("shader_cache"))]
        Shader_cache,
        /// <summary>
        /// websql
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("websql"))]
        Websql,
        /// <summary>
        /// service_workers
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("service_workers"))]
        Service_workers,
        /// <summary>
        /// cache_storage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cache_storage"))]
        Cache_storage,
        /// <summary>
        /// all
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("all"))]
        All,
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other
    }
}