// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the FrameLoadStart event handler set up in IWebBrowser.
    /// </summary>
    public class FrameLoadStartEventArgs : EventArgs
    {
        public FrameLoadStartEventArgs(string url, bool isMainFrame)
        {
            Url = url;
            IsMainFrame = isMainFrame;
        }

        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Is this the Main Frame
        /// </summary>
        public bool IsMainFrame { get; private set; }
    }
}
