// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Experimental
{
    /// <summary>
    /// Provides a convenient <see cref="NativeWindow"/> implement
    /// that can be used without having to create your own class
    /// </summary>
    public class ChromiumWidgetNativeWindow : NativeWindow
    {
        private Func<Message, bool> wndProcHandler;

        /// <summary>
        /// ChromiumWidgetMessageInterceptor constructor
        /// </summary>
        /// <param name="control">Control is used to handled the <see cref="Control.HandleDestroyed"/> event so
        /// we can automatically call <see cref="NativeWindow.ReleaseHandle"/>. If null then you are responsible
        /// for calling <see cref="NativeWindow.ReleaseHandle"/></param>
        /// <param name="chromeWidgetHostHandle">Hwnd to intercept messages for.</param>
        public ChromiumWidgetNativeWindow(Control control, IntPtr chromeWidgetHostHandle)
        {
            AssignHandle(chromeWidgetHostHandle);

            if (control != null)
            {
                control.HandleDestroyed += BrowserHandleDestroyed;
            }
        }

        private void BrowserHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();

            var control = (Control)sender;

            control.HandleDestroyed -= BrowserHandleDestroyed;
            wndProcHandler = null;
        }

        /// <summary>
        /// Register a Func which is used to intercept <see cref="WndProc(ref Message)"/>
        /// calls. <paramref name="wndProcHandler"/> should return true if the message
        /// was handled, otherwise false.
        /// </summary>
        /// <param name="wndProcHandler">Func to be used to intercept messages, null to clear an existing function.</param>
        public void OnWndProc(Func<Message, bool> wndProcHandler)
        {
            this.wndProcHandler = wndProcHandler;
        }

        /// <inheritdoc/>
        protected override void WndProc(ref Message m)
        {
            var handler = wndProcHandler;

            var handled = handler?.Invoke(m);

            if (handled == false)
            {
                base.WndProc(ref m);
            }
        }
    }
}
