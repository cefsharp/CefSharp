// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public abstract class BaseRequestEventArgs : System.EventArgs
    {
        protected BaseRequestEventArgs(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            ChromiumWebBrowser = chromiumWebBrowser;
            Browser = browser;
        }

        public IWebBrowser ChromiumWebBrowser { get; private set; }
        public IBrowser Browser { get; private set; }
    }
}
