﻿using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the FrameLoadEnd event handler set up in IWebBrowser.
    /// </summary>
    public class FrameLoadEndEventArgs : EventArgs
    {
        public FrameLoadEndEventArgs(string url, bool isMainFrame, int httpStatusCode)
        {
            Url = url;
            IsMainFrame = isMainFrame;
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Is this the Main Frame
        /// </summary>
        public bool IsMainFrame { get; private set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public int HttpStatusCode { get; set; }
    };

    /// <summary>
    /// A delegate type used to listen to FrameLoadEnd events.
    /// </summary>
    public delegate void FrameLoadEndEventHandler(object sender, FrameLoadEndEventArgs url);
}
