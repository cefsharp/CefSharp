// Copyright © 2014 The CefSharp Authors. All rights reserved.
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
        /// <summary>
        /// Creates a new FrameLoadStart event args
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="frame">frame</param>
        /// <param name="transitionType"> provides information about the source of the navigation and an accurate value is only
        /// available in the browser process</param>
        public FrameLoadStartEventArgs(IBrowser browser, IFrame frame, TransitionType transitionType)
        {
            Browser = browser;
            Frame = frame;
            if (frame.IsValid)
            {
                Url = frame.Url;
            }
            TransitionType = transitionType;
        }

        /// <summary>
        /// The browser object
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// The frame that just started loading.
        /// </summary>
        public IFrame Frame { get; private set; }

        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// TransitionType provides information about the source of the navigation.
        /// </summary>
        public TransitionType TransitionType { get; private set; }
    }
}
