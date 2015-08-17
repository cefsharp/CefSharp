// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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

        public AddressChangedEventArgs(IBrowser browser, string address)
        {
            Browser = browser;
            Address = address;
        }
    }
}
