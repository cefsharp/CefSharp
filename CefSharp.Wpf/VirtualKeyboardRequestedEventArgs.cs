// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Event arguments for the VirtualKeyboardRequested Event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class VirtualKeyboardRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Input mode of a virtual keyboard. When <see cref="TextInputMode.None"/>
        /// the keyboard should be hidden
        /// </summary>
        public TextInputMode TextInputMode { get; private set; }

        /// <summary>
        /// Browser
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualKeyboardRequestedEventArgs"/> class.
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="inputMode">input mode</param>
        public VirtualKeyboardRequestedEventArgs(IBrowser browser, TextInputMode inputMode)
        {
            Browser = browser;
            TextInputMode = inputMode;
        }
    }
}
