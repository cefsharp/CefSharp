// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    /// <summary>
    /// Data entry.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DataEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Request URL.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestURL"), IsRequired = (true))]
        public string RequestURL
        {
            get;
            set;
        }

        /// <summary>
        /// Request method.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestMethod"), IsRequired = (true))]
        public string RequestMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Request headers
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestHeaders"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Header> RequestHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// Number of seconds since epoch.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseTime"), IsRequired = (true))]
        public long ResponseTime
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseStatus"), IsRequired = (true))]
        public int ResponseStatus
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseStatusText"), IsRequired = (true))]
        public string ResponseStatusText
        {
            get;
            set;
        }

        public CefSharp.DevTools.CacheStorage.CachedResponseType ResponseType
        {
            get
            {
                return (CefSharp.DevTools.CacheStorage.CachedResponseType)(StringToEnum(typeof(CefSharp.DevTools.CacheStorage.CachedResponseType), responseType));
            }

            set
            {
                responseType = (EnumToString(value));
            }
        }

        /// <summary>
        /// HTTP response type
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseType"), IsRequired = (true))]
        internal string responseType
        {
            get;
            set;
        }

        /// <summary>
        /// Response headers
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseHeaders"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Header> ResponseHeaders
        {
            get;
            set;
        }
    }
}