// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the StatusMessage event handler set up in IWebBrowser.
    /// </summary>
    public class StatusMessageEventArgs : EventArgs
    {
        public StatusMessageEventArgs(IBrowser browser, string value)
        {
            Browser = browser;
            Value = value;
        }

        /// <summary>
        /// The browser object
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// The value of the status message.
        /// </summary>
        public string Value { get; private set; }
    }
}
