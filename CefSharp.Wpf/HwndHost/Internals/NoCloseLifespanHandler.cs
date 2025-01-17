// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf.HwndHost.Internals
{
    /// <summary>
    /// LifeSpanHandler used internally
    /// - Cancels sending of WM_CLOSE message for main browser
    /// - Allows popups to close
    /// </summary>
    public class NoCloseLifespanHandler
        : CefSharp.Handler.LifeSpanHandler
    {
        /// <inheritdoc/>
        protected override bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if(browser.IsPopup)
            {
                return false;
            }

            return true;
        }
    }
}
