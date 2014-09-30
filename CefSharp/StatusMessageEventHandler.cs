// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
        public StatusMessageEventArgs(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the status message.
        /// </summary>
        public string Value { get; private set; }
    }
}
