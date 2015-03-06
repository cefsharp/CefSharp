// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using System.Windows.Forms;
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
            if (browser.activating)
            {
                browser.activating = false;
                Kernel32.OutputDebugString("BrowserOnGotFocus: during activate, doing nothing, WinForms .ActiveControl processing will correct if necessary.\r\n");
            }
            else
            {
                Kernel32.OutputDebugString("DFH: Queuing .Activate() call\r\n");
                browser.InvokeOnUiThreadIfRequired(() =>
                {
                    Kernel32.OutputDebugString("Before browser.Activate()\r\n");
                    Kernel32.OutputDebugString("Activate result: " + browser.Activate().ToString() + "\r\n");
                    Kernel32.OutputDebugString("POST .Activate()\r\n");
                    Kernel32.OutputDebugString(String.Format("CCActiveControlType: {0}\r\n", ((IContainerControl)browser.Parent).ActiveControl.GetType().FullName));
                    Kernel32.OutputDebugString(String.Format("CCActiveControlType: {0}\r\n", ((IContainerControl)browser.FindForm()).ActiveControl.GetType().FullName));
                });
            }
        }

        public virtual bool OnSetFocus(CefFocusSource source)
        {
            Kernel32.OutputDebugString(String.Format("Focus Handler:: OnSetFocus {0}\r\n", source));
            // Do not let the browser take focus when a Load method has been called
            return source == CefFocusSource.FocusSourceNavigation;
        }

        public virtual void OnTakeFocus(bool next)
        {
            Kernel32.OutputDebugString(String.Format("Focus Handler:: OnTakeFocus {0}\r\n", next));
            // NOTE: OnTakeFocus means leaving focus / not taking focus
            browser.InvokeOnUiThreadIfRequired(() => browser.SelectNextControl(next));
        }
    }
}
