// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments for the IsBrowserInitializedChanged event handler.
    /// </summary>
    public class IsBrowserInitializedChangedEventArgs : EventArgs
    {
        public bool IsBrowserInitialized { get; private set; }

        public IsBrowserInitializedChangedEventArgs(bool isBrowserInitialized)
        {
            IsBrowserInitialized = isBrowserInitialized;
        }
    }
}
