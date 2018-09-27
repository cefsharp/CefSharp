// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments for the IsBrowserInitializedChanged event handler.
    /// TODO: Remove this event args, we don't actually use it (event is only fired once)
    /// </summary>
    public class IsBrowserInitializedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Is browser initialized
        /// </summary>
        public bool IsBrowserInitialized { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="isBrowserInitialized">bool</param>
        public IsBrowserInitializedChangedEventArgs(bool isBrowserInitialized)
        {
            IsBrowserInitialized = isBrowserInitialized;
        }
    }
}
