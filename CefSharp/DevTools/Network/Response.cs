// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// HTTP response data.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Response : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Response URL. This URL can be different from CachedResource.url in case of redirect.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("status"), IsRequired = (true))]
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response status text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("statusText"), IsRequired = (true))]
        public string StatusText
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headers"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Headers Headers
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP response headers text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headersText"), IsRequired = (false))]
        public string HeadersText
        {
            get;
            set;
        }

        /// <summary>
        /// Resource mimeType as determined by the browser.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mimeType"), IsRequired = (true))]
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Refined HTTP request headers that were actually transmitted over the network.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestHeaders"), IsRequired = (false))]
        public CefSharp.DevTools.Network.Headers RequestHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestHeadersText"), IsRequired = (false))]
        public string RequestHeadersText
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies whether physical connection was actually reused for this request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("connectionReused"), IsRequired = (true))]
        public bool ConnectionReused
        {
            get;
            set;
        }

        /// <summary>
        /// Physical connection id that was actually used for this request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("connectionId"), IsRequired = (true))]
        public long ConnectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Remote IP address.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("remoteIPAddress"), IsRequired = (false))]
        public string RemoteIPAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Remote port.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("remotePort"), IsRequired = (false))]
        public int? RemotePort
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies that the request was served from the disk cache.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fromDiskCache"), IsRequired = (false))]
        public bool? FromDiskCache
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies that the request was served from the ServiceWorker.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fromServiceWorker"), IsRequired = (false))]
        public bool? FromServiceWorker
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies that the request was served from the prefetch cache.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fromPrefetchCache"), IsRequired = (false))]
        public bool? FromPrefetchCache
        {
            get;
            set;
        }

        /// <summary>
        /// Total number of bytes received for this request so far.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("encodedDataLength"), IsRequired = (true))]
        public long EncodedDataLength
        {
            get;
            set;
        }

        /// <summary>
        /// Timing information for the given request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timing"), IsRequired = (false))]
        public CefSharp.DevTools.Network.ResourceTiming Timing
        {
            get;
            set;
        }

        /// <summary>
        /// Response source of response from ServiceWorker.
        /// </summary>
        public CefSharp.DevTools.Network.ServiceWorkerResponseSource? ServiceWorkerResponseSource
        {
            get
            {
                return (CefSharp.DevTools.Network.ServiceWorkerResponseSource? )(StringToEnum(typeof(CefSharp.DevTools.Network.ServiceWorkerResponseSource? ), serviceWorkerResponseSource));
            }

            set
            {
                serviceWorkerResponseSource = (EnumToString(value));
            }
        }

        /// <summary>
        /// Response source of response from ServiceWorker.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("serviceWorkerResponseSource"), IsRequired = (false))]
        internal string serviceWorkerResponseSource
        {
            get;
            set;
        }

        /// <summary>
        /// The time at which the returned response was generated.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseTime"), IsRequired = (false))]
        public long? ResponseTime
        {
            get;
            set;
        }

        /// <summary>
        /// Cache Storage Cache Name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cacheStorageCacheName"), IsRequired = (false))]
        public string CacheStorageCacheName
        {
            get;
            set;
        }

        /// <summary>
        /// Protocol used to fetch this request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("protocol"), IsRequired = (false))]
        public string Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Security state of the request resource.
        /// </summary>
        public CefSharp.DevTools.Security.SecurityState SecurityState
        {
            get
            {
                return (CefSharp.DevTools.Security.SecurityState)(StringToEnum(typeof(CefSharp.DevTools.Security.SecurityState), securityState));
            }

            set
            {
                securityState = (EnumToString(value));
            }
        }

        /// <summary>
        /// Security state of the request resource.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("securityState"), IsRequired = (true))]
        internal string securityState
        {
            get;
            set;
        }

        /// <summary>
        /// Security details for the request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("securityDetails"), IsRequired = (false))]
        public CefSharp.DevTools.Network.SecurityDetails SecurityDetails
        {
            get;
            set;
        }
    }
}