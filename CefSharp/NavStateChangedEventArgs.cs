// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the NavStateChanged event handler set up in IWebBrowser.
    /// </summary>
    public class NavStateChangedEventArgs : EventArgs
    {
        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }

        public NavStateChangedEventArgs(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;
        }
    }
}
