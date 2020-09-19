// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Timing information for the request.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ResourceTiming : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Timing's requestTime is a baseline in seconds, while the other numbers are ticks in
        /// milliseconds relatively to this requestTime.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestTime"), IsRequired = (true))]
        public long RequestTime
        {
            get;
            set;
        }

        /// <summary>
        /// Started resolving proxy.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("proxyStart"), IsRequired = (true))]
        public long ProxyStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished resolving proxy.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("proxyEnd"), IsRequired = (true))]
        public long ProxyEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started DNS address resolve.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("dnsStart"), IsRequired = (true))]
        public long DnsStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished DNS address resolve.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("dnsEnd"), IsRequired = (true))]
        public long DnsEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started connecting to the remote host.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("connectStart"), IsRequired = (true))]
        public long ConnectStart
        {
            get;
            set;
        }

        /// <summary>
        /// Connected to the remote host.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("connectEnd"), IsRequired = (true))]
        public long ConnectEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started SSL handshake.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sslStart"), IsRequired = (true))]
        public long SslStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished SSL handshake.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sslEnd"), IsRequired = (true))]
        public long SslEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started running ServiceWorker.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("workerStart"), IsRequired = (true))]
        public long WorkerStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished Starting ServiceWorker.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("workerReady"), IsRequired = (true))]
        public long WorkerReady
        {
            get;
            set;
        }

        /// <summary>
        /// Started fetch event.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("workerFetchStart"), IsRequired = (true))]
        public long WorkerFetchStart
        {
            get;
            set;
        }

        /// <summary>
        /// Settled fetch event respondWith promise.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("workerRespondWithSettled"), IsRequired = (true))]
        public long WorkerRespondWithSettled
        {
            get;
            set;
        }

        /// <summary>
        /// Started sending request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sendStart"), IsRequired = (true))]
        public long SendStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished sending request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sendEnd"), IsRequired = (true))]
        public long SendEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Time the server started pushing request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pushStart"), IsRequired = (true))]
        public long PushStart
        {
            get;
            set;
        }

        /// <summary>
        /// Time the server finished pushing request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pushEnd"), IsRequired = (true))]
        public long PushEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Finished receiving response headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("receiveHeadersEnd"), IsRequired = (true))]
        public long ReceiveHeadersEnd
        {
            get;
            set;
        }
    }
}