using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadCompleted event handler set up in IWebBrowser.
    /// </summary>
    public class LoadCompletedEventArgs : EventArgs
    {
        public LoadCompletedEventArgs(string url)
        {
            Url = Url;
        }

        /// <summary>
        /// The URL that was loaded.
        /// </summary>
        public string Url { get; private set; }
    };

    /// <summary>
    /// A delegate type used to listen to LoadCompleted events.
    /// </summary>
    public delegate void LoadCompletedEventHandler(object sender, LoadCompletedEventArgs url);
}
