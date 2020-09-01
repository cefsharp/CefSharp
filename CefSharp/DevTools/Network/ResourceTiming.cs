// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Timing information for the request.
    /// </summary>
    public class ResourceTiming
    {
        /// <summary>
        /// Timing's requestTime is a baseline in seconds, while the other numbers are ticks in
        public long RequestTime
        {
            get;
            set;
        }

        /// <summary>
        /// Started resolving proxy.
        /// </summary>
        public long ProxyStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished resolving proxy.
        /// </summary>
        public long ProxyEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started DNS address resolve.
        /// </summary>
        public long DnsStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished DNS address resolve.
        /// </summary>
        public long DnsEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started connecting to the remote host.
        /// </summary>
        public long ConnectStart
        {
            get;
            set;
        }

        /// <summary>
        /// Connected to the remote host.
        /// </summary>
        public long ConnectEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started SSL handshake.
        /// </summary>
        public long SslStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished SSL handshake.
        /// </summary>
        public long SslEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Started running ServiceWorker.
        /// </summary>
        public long WorkerStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished Starting ServiceWorker.
        /// </summary>
        public long WorkerReady
        {
            get;
            set;
        }

        /// <summary>
        /// Started fetch event.
        /// </summary>
        public long WorkerFetchStart
        {
            get;
            set;
        }

        /// <summary>
        /// Settled fetch event respondWith promise.
        /// </summary>
        public long WorkerRespondWithSettled
        {
            get;
            set;
        }

        /// <summary>
        /// Started sending request.
        /// </summary>
        public long SendStart
        {
            get;
            set;
        }

        /// <summary>
        /// Finished sending request.
        /// </summary>
        public long SendEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Time the server started pushing request.
        /// </summary>
        public long PushStart
        {
            get;
            set;
        }

        /// <summary>
        /// Time the server finished pushing request.
        /// </summary>
        public long PushEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Finished receiving response headers.
        /// </summary>
        public long ReceiveHeadersEnd
        {
            get;
            set;
        }
    }
}