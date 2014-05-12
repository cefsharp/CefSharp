using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the AddressChanged event handler.
    /// </summary>
    public class AddressChangedEventArgs : EventArgs
    {
        public string Address { get; private set; }

        public AddressChangedEventArgs(string address)
        {
            Address = address;
        }
    };

    /// <summary>
    /// A delegate type used to listen to AddressChanged events.
    /// </summary>
    public delegate void AddressChangedEventHandler(object sender, AddressChangedEventArgs args);
}
