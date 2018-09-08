// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments for the AddressChanged event handler.
    /// </summary>
    public class AddressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Access to the underlying <see cref="IBrowser"/> object
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// The new address
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Creates a new AddressChangedEventArgs event argument.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="address">the address</param>
        public AddressChangedEventArgs(IBrowser browser, string address)
        {
            Browser = browser;
            Address = address;
        }
    }
}
