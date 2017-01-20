// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnQuotaRequestEventArgs : BaseRequestEventArgs
    {
        public OnQuotaRequestEventArgs(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            : base(browserControl, browser)
        {
            OriginUrl = originUrl;
            NewSize = newSize;
            Callback = callback;
        }

        public string OriginUrl { get; }
        public long NewSize { get; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true to continue the request
        ///     and call <see cref="T:OnQuotaRequestEventArgs.Callback.Continue(System.Boolean)" /> either in this method or at a later
        ///     time to grant or deny the request.
        /// </summary>
        public bool ContinueAsync { get; set; } = false;
    }
}
