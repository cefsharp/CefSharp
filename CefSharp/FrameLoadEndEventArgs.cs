// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the FrameLoadEnd event handler set up in IWebBrowser.
    /// </summary>
    public class FrameLoadEndEventArgs : EventArgs
    {
        public FrameLoadEndEventArgs(IFrame frame, int httpStatusCode)
        {
            Frame = frame;
            Url = frame.GetUrl();
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// The frame that finished loading.
        /// </summary>
        public IFrame Frame { get; private set; }
        
        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public int HttpStatusCode { get; set; }
    }
}
