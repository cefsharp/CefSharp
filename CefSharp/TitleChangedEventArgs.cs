using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the TitleChanged event handler.
    /// </summary>
    public class TitleChangedEventArgs : EventArgs
    {
        public string Title { get; private set; }

        public TitleChangedEventArgs(string title)
        {
            Title = title;
        }
    };

    /// <summary>
    /// A delegate type used to listen to TitleChanged events.
    /// </summary>
    public delegate void TitleChangedEventHandler(object sender, TitleChangedEventArgs args);
}
