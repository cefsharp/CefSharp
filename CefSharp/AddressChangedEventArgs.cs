// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the AddressChanged event handler.
    /// </summary>
    public class AddressChangedEventArgs : EventArgs
    {
        public IBrowser Browser { get; set; }
        public string Address { get; private set; }

        /// <summary>
        /// Called when a frame's address has changed. 
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
