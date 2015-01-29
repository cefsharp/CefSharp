// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms.Internals
{
    internal class DefaultFocusHandler : IFocusHandler
    {
        private readonly ChromiumWebBrowser browser;

        public DefaultFocusHandler(ChromiumWebBrowser browser)
        {
            this.browser = browser;
        }

        public virtual void OnGotFocus()
        {
            browser.InvokeOnUiThreadIfRequired(browser.Activate);
        }

        public virtual bool OnSetFocus(CefFocusSource source)
        {
            //Do not let the browser take focus when a Load method has been called
            return source == CefFocusSource.FocusSourceNavigation;
        }

        public virtual void OnTakeFocus(bool next)
        {
            //NOTE: OnTakeFocus means leaving focus / not taking focus
            browser.InvokeOnUiThreadIfRequired(() => browser.SelectNextControl(next));
        }
    }
}
