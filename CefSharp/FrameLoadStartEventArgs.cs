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
    };

    /// <summary>
    /// A delegate type used to listen to FrameLoadStart events.
    /// </summary>
    public delegate void FrameLoadStartEventHandler(object sender, FrameLoadStartEventArgs args);
}
