// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.WinForms.Host
{
    /// <summary>
    /// Chromium Browser Host Control, provides base functionality for hosting a
    /// CefBrowser instance (main browser and popups) in WinForms.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    public abstract class ChromiumHostControlBase : Control
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// IntPtr that represents the CefBrowser Hwnd
        /// Used for sending messages to the browser
        /// e.g. resize
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr BrowserHwnd { get; set; }
        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        /// <value><c>true</c> if this instance is activating; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsActivating { get; set; }

        /// <summary>
        /// Event called after the underlying CEF browser instance has been created. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler IsBrowserInitializedChanged;

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <value>
        /// The default <see cref="T:System.Drawing.Size" /> of the control.
        /// </value>
        protected override Size DefaultSize
        {
            get { return new Size(200, 100); }
        }

        /// <summary>
        /// Makes certain keys as Input keys when CefSettings.MultiThreadedMessageLoop = false
        /// </summary>
        /// <param name="keyData">key data</param>
        /// <returns>true for a select list of keys otherwise defers to base.IsInputKey</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            //This code block is only called/required when CEF is running in the
            //same message loop as the WinForms UI (CefSettings.MultiThreadedMessageLoop = false)
            //Without this code, arrows and tab won't be processed
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Tab:
                {
                    return true;
                }
                case Keys.Shift | Keys.Tab:
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                {
                    return true;
                }
            }

            return base.IsInputKey(keyData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            ResizeBrowser(Width, Height);

            base.OnSizeChanged(e);
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                ShowInternal();
            }
            else
            {
                HideInternal();
            }

            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Resizes the browser to the specified <paramref name="width"/> and <paramref name="height"/>.
        /// If <paramref name="width"/> and <paramref name="height"/> are both 0 then the browser
        /// will be hidden and resource usage will be minimised.
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        protected virtual void ResizeBrowser(int width, int height)
        {
            if (BrowserHwnd != IntPtr.Zero)
            {
                SetWindowPosition(BrowserHwnd, 0, 0, width, height);
            }
        }

        /// <summary>
        /// When minimized set the browser window size to 0x0 to reduce resource usage.
        /// https://github.com/chromiumembedded/cef/blob/c7701b8a6168f105f2c2d6b239ce3958da3e3f13/tests/cefclient/browser/browser_window_std_win.cc#L87
        /// </summary>
        internal virtual void HideInternal()
        {
            if (BrowserHwnd != IntPtr.Zero)
            {
                SetWindowPosition(BrowserHwnd, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Show the browser (called after previous minimised)
        /// </summary>
        internal virtual void ShowInternal()
        {
            if (BrowserHwnd != IntPtr.Zero)
            {
                SetWindowPosition(BrowserHwnd, 0, 0, Width, Height);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BrowserHwnd = IntPtr.Zero;
                IsBrowserInitializedChanged = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Trigger the <see cref="IsBrowserInitializedChanged"/> event
        /// </summary>
        internal void RaiseIsBrowserInitializedChangedEvent()
        {
            IsBrowserInitializedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetWindowPosition(IntPtr handle, int x, int y, int width, int height)
        {
            const uint SWP_NOMOVE = 0x0002;
            const uint SWP_NOZORDER = 0x0004;
            const uint SWP_NOACTIVATE = 0x0010;

            if (handle != IntPtr.Zero)
            {
                if (width == 0 && height == 0)
                {
                    // For windowed browsers when the frame window is minimized set the
                    // browser window size to 0x0 to reduce resource usage.
                    SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOACTIVATE);
                }
                else
                {
                    SetWindowPos(handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ChromiumHostControl"/> or <see cref="ChromiumWebBrowser"/> associated with
        /// a specific <see cref="IBrowser"/> instance. 
        /// </summary>
        /// <param name="browser">browser</param>
        /// <returns>returns the assocaited <see cref="ChromiumHostControl"/> or <see cref="ChromiumWebBrowser"/> or null if Disposed or no host found.</returns>
        public static T FromBrowser<T>(IBrowser browser) where T : ChromiumHostControlBase
        {
            if (browser.IsDisposed)
            {
                return null;
            }

            var windowHandle = browser.GetHost().GetWindowHandle();

            if (windowHandle == IntPtr.Zero)
            {
                return null;
            }

            var control = Control.FromChildHandle(windowHandle) as T;

            return control;
        }
    }
}
