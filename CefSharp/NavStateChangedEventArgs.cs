using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the NavStateChanged event handler set up in IWebBrowser.
    /// </summary>
    public class NavStateChangedEventArgs : EventArgs
    {
        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }

        public NavStateChangedEventArgs(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;
        }
    };

    /// <summary>
    /// A delegate type used to listen to NavStateChanged events.
    /// </summary>
    public delegate void NavStateChangedEventHandler(object sender, NavStateChangedEventArgs args);
}
