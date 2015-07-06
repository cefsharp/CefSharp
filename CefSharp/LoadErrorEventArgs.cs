// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadError event handler set up in IWebBrowser.
    /// </summary>
    public class LoadErrorEventArgs : EventArgs
    {
        public LoadErrorEventArgs(IFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            Frame = frame;
            ErrorCode = errorCode;
            ErrorText = errorText;
            FailedUrl = failedUrl;
        }

        /// <summary>
        /// The frame that failed to load.
        /// </summary>
        public IFrame Frame { get; private set; }

        /// <summary>
        /// The URL that failed to load.
        /// </summary>
        public string FailedUrl { get; private set; }

        /// <summary>
        /// The error code.
        /// </summary>
        public CefErrorCode ErrorCode { get; private set; }

        /// <summary>
        /// The error text.
        /// </summary>
        public string ErrorText { get; private set; }
    }
}
