// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnResourceRedirectEventArgs : BaseRequestEventArgs
    {
        public OnResourceRedirectEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, string newUrl)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;
            NewUrl = newUrl;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }

        /// <summary>
        ///     the new URL and can be changed if desired.
        /// </summary>
        public string NewUrl { get; set; }
    }
}
