// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using System.Windows.Forms;
namespace CefSharp.WinForms.Internals
{
    public class DefaultFocusHandler : IFocusHandler
    {
        private readonly ChromiumWebBrowser browser;

        public DefaultFocusHandler(ChromiumWebBrowser browser)
        {
            this.browser = browser;
        }

        /// <remarks>
        /// Try to avoid needing to override this logic in a subclass. The implementation in 
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and 
        /// Windows interact during window activation.
        /// </remarks>
        public virtual void OnGotFocus()
        {
            // During application activation, CEF receives a WM_SETFOCUS
            // message from Windows because it is the top window 
            // on the CEF UI thread.
            //
            // If the WinForm ChromiumWebBrowser control is the 
            // current .ActiveControl before app activation 
            // then we MUST NOT try to reactivate the WinForm
            // control during activation because that will 
            // start a race condition between reactivating
            // the CEF control AND having another control 
            // that should be the new .ActiveControl.
            //
            // For example:
            // * CEF control has focus, and thus ChromiumWebBrowser
            //   is the current .ActiveControl
            // * Alt-Tab to another application
            // * Click a non CEF control in the WinForms application.
            // * This begins the Windows activation process.
            // * The WM_ACTIVATE process on the WinForm UI thread
            //   will update .ActiveControl to the clicked control.
            //   The clicked control will receive WM_SETFOCUS as well. 
            //   (i.e. OnGotFocus)
            //   If the ChromiumWebBrowser was the previous .ActiveControl,
            //   then we set .Activating = true.
            // * The WM_ACTIVATE process on the CEF thread will
            //   send WM_SETFOCUS to CEF thus staring the race of
            //   which will end first, the WndProc WM_ACTIVATE process
            //   on the WinForm UI thread or the WM_ACTIVATE process
            //   on the CEF UI thread.
            // * CEF will then call this method on the CEF UI thread
            //   due to WM_SETFOCUS.
            // * This method will clear the activation state (if any)
            //   on the ChromiumWebBrowser control, due to the race
            //   condition the WinForm UI thread cannot.
            if (browser.IsActivating)
            {
                browser.IsActivating = false;
            }
            else
            {
                // Otherwise, we're not being activated
                // so we must activate the ChromiumWebBrowser control
                // for WinForms focus tracking.
                browser.InvokeOnUiThreadIfRequired(() =>
                {
                    browser.Activate();
                });
            }
        }

        public virtual bool OnSetFocus(CefFocusSource source)
        {
            // Do not let the browser take focus when a Load method has been called
            return source == CefFocusSource.FocusSourceNavigation;
        }

        public virtual void OnTakeFocus(bool next)
        {
            // NOTE: OnTakeFocus means leaving focus / not taking focus
            browser.InvokeOnUiThreadIfRequired(() => browser.SelectNextControl(next));
        }
    }
}
