// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnQuotaRequestEventArgs : BaseRequestEventArgs
    {
        public OnQuotaRequestEventArgs(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            : base(chromiumWebBrowser, browser)
        {
            OriginUrl = originUrl;
            NewSize = newSize;
            Callback = callback;

            ContinueAsync = false; // default
        }

        public string OriginUrl { get; private set; }
        public long NewSize { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; private set; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true to continue the request
        ///     and call <see cref="T:OnQuotaRequestEventArgs.Callback.Continue(System.Boolean)" /> either in this method or at a later
        ///     time to grant or deny the request.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }
}
