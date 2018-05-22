// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class CanGetCookiesEventArg : BaseRequestEventArgs
    {
        public CanGetCookiesEventArg(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public bool IsRedirect { get; private set; }

        /// <summary>
        /// Return true to allow cookies to be included in the network request or false to block cookies.
        /// </summary>
        public bool GetCookies { get; set; } = true;
    }
}
