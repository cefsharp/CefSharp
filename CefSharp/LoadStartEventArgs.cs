using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadStart event handler set up in IWebBrowser.
    /// </summary>
    public class LoadStartEventArgs : EventArgs
    {
        public LoadStartEventArgs(string url, bool isMainFrame)
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
    /// A delegate type used to listen to LoadStart events.
    /// </summary>
    public delegate void LoadStartEventHandler(object sender, LoadStartEventArgs args);
}
