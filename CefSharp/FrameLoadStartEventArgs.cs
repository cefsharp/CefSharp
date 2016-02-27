﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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
        public FrameLoadStartEventArgs(IBrowser browser, IFrame frame)
        {
            Browser = browser;
            Frame = frame;
            Url = frame.Url;
        }

        /// <summary>
        /// The browser object
        /// </summary>
        public IBrowser Browser { get; private set;}

        /// <summary>
        /// The frame that just started loading.
        /// </summary>
        public IFrame Frame { get; private set; }
        
        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }
    }
}
