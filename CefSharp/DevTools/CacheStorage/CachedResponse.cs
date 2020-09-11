// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// Cached response
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CachedResponse : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Entry content, base64-encoded.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("body"), IsRequired = (true))]
        public byte[] Body
        {
            get;
            set;
        }
    }
}