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
            browser.InvokeOnUiThreadIfRequired(() =>
            {
                browser.Activate();
                Kernel32.OutputDebugString("POST .Activate()\r\n");
                Kernel32.OutputDebugString(String.Format("CCActiveControlType: {0}\r\n", ((IContainerControl)browser.Parent).ActiveControl.GetType().FullName));
                Kernel32.OutputDebugString(String.Format("CCActiveControlType: {0}\r\n", ((IContainerControl)browser.FindForm()).ActiveControl.GetType().FullName));
            });
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
