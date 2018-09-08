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
        /// The new title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Creates a new TitleChanged event arg
        /// </summary>
        /// <param name="title">the new title</param>
        public TitleChangedEventArgs(string title)
        {
            Title = title;
        }
    }
}
