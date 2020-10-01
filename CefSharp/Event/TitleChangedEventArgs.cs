// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the TitleChanged event handler.
    /// </summary>
    public class TitleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Access to the underlying <see cref="IBrowser"/> object
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        /// The new title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Creates a new TitleChanged event arg
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="title">the new title</param>
        public TitleChangedEventArgs(IBrowser browser, string title)
        {
            Browser = browser;
            Title = title;
        }
    }
}
