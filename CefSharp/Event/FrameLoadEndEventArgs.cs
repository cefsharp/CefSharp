// Copyright © 2014 The CefSharp Authors. All rights reserved.
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
        /// <summary>
        /// Creates a new FrameLoadEnd event args
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="frame">frame</param>
        /// <param name="httpStatusCode">http statusCode</param>
        public FrameLoadEndEventArgs(IBrowser browser, IFrame frame, int httpStatusCode)
        {
            Browser = browser;
            Frame = frame;
            if (frame.IsValid)
            {
                Url = frame.Url;
            }
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// The browser that contains the frame that finished loading.
        /// </summary>
        public IBrowser Browser { get; private set; }

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
