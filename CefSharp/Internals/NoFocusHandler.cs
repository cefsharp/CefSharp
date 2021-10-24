// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    /// <summary>
    /// NoFocusHandler - Used when disposing of the ChromiumWebBrowser controls
    /// Doesn't take focus for the main browser (leaves default behaviour for popup).
    /// OnGotFocus and OnTakeFocus are both noops.
    /// </summary>
    public class NoFocusHandler : IFocusHandler
    {
        void IFocusHandler.OnGotFocus(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //NOOP   
        }

        bool IFocusHandler.OnSetFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFocusSource source)
        {
            if(browser.IsPopup)
            {
                return false;
            }

            //Don't take focus
            return true;
        }

        void IFocusHandler.OnTakeFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, bool next)
        {
            //NOOP
        }
    }
}
