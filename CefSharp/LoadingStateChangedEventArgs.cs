// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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
        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }
        public bool IsLoading { get; private set; }
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
