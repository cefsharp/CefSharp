// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class GetAuthCredentialsEventArgs : BaseRequestEventArgs
    {
        public GetAuthCredentialsEventArgs(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) : base(chromiumWebBrowser, browser)
        {
            OriginUrl = originUrl;
            IsProxy = isProxy;
            Host = host;
            Port = port;
            Realm = realm;
            Scheme = scheme;
            Callback = callback;

            ContinueAsync = false; // default
        }

        public string OriginUrl { get; private set; }
        public bool IsProxy { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Realm { get; private set; }
        public string Scheme { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of authentication requests.
        /// </summary>
        public IAuthCallback Callback { get; private set; }

        /// <summary>
        ///     Set to true to continue the request and call
        ///     <see cref="T:CefSharp.GetAuthCredentialsEventArgs.Continue(System.String, System.String)" /> when the authentication information
        ///     is available. Set to false to cancel the request.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }
}
