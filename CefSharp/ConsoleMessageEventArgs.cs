using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the ConsoleMessage event handler set up in IWebBrowser.
    /// </summary>
    public class ConsoleMessageEventArgs : EventArgs
    {
        public ConsoleMessageEventArgs(string message, string source, int line)
        {
            Message = message;
            Source = source;
            Line = line;
        }

        /// <summary>
        /// The message text of the console message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The source of the console message.
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// The line number that generated the console message.
        /// </summary>
        public int Line { get; private set; }
    }

    /// <summary>
    /// A delegate type used to listen to ConsoleMessage events.
    /// </summary>
    public delegate void ConsoleMessageEventHandler(object sender, ConsoleMessageEventArgs e);
}
