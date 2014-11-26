// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    internal class FocusHandler : IFocusHandler
    {
        private readonly ChromiumWebBrowser browser;
        private readonly ToolStripTextBox urlTextBox;

        public FocusHandler(ChromiumWebBrowser browser, ToolStripTextBox urlTextBox)
        {
            this.browser = browser;
            this.urlTextBox = urlTextBox;
        }

        public void OnGotFocus()
        {
            // Fired when Chromium finally receives focus.
        }

        public bool OnSetFocus(CefFocusSource source)
        {
            // Returning false means that we allow Chromium to take the focus away from our WinForms control.
            // You could also return true to keep focus in the address bar.
            return false;
        }

        public void OnTakeFocus(bool next)
        {
            // Fired when Chromium is giving up focus because the user
            // pressed Tab on the last link/field in the HTML document (in which case 'next' is true)
            // or because they pressed Shift+Tab on the first link/field (in which case 'next' is false).
            
            // Here we always focus on the address bar.
            urlTextBox.Control.BeginInvoke(new MethodInvoker(() => urlTextBox.Focus()));
        }
    }
}