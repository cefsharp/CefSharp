// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadingStateChanged event handler set up in IWebBrowser.
    /// </summary>
    public class LoadingStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if the browser can navigate forwards. 
        /// </summary>
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// Returns true if the browser can navigate backwards. 
        /// </summary>
        public bool CanGoBack { get; private set; }
        /// <summary>
        /// Returns true if the browser can reload. 
        /// </summary>
        public bool CanReload { get; private set; }
        /// <summary>
        /// Returns true if the browser is loading. 
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Access to the underlying <see cref="IBrowser"/> object
        /// </summary>
        public IBrowser Browser { get; private set; }

        public LoadingStateChangedEventArgs(IBrowser browser, bool canGoBack, bool canGoForward, bool isLoading)
        {
            Browser = browser;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            IsLoading = isLoading;
            CanReload = !isLoading;
        }
    }
}
