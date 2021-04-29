// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    /// <summary>
    /// Chromium Browser Host Control, provides base functionality for hosting a
    /// CefBrowser instance (main browser and popups) in  WinForms
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    [Docking(DockingBehavior.AutoDock), ToolboxBitmap(typeof(ChromiumWebBrowser)),
    Designer(typeof(ChromiumWebBrowserDesigner))]
    public class ChromiumHostControl : Control
    {
        /// <summary>
        /// IntPtr that represents the CefBrowser Hwnd
        /// Used for sending messages to the browser
        /// e.g. resize
        /// </summary>
        public IntPtr BrowserHwnd { get; internal set; }
        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        /// <value><c>true</c> if this instance is activating; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsActivating { get; set; }

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
            base.OnSizeChanged(e);

            //TODO: Currently ChromiumWebBrowser has it's own resize,
            //we should should consolidate this, for now they're seperate
            //(Though they can be identical in reality)
            if (GetType() != typeof(ChromiumWebBrowser))
            {
                if (BrowserHwnd != IntPtr.Zero)
                {
                    ResizeBrowser(Width, Height);
                }
            }
        }

        /// <summary>
        /// Resizes the browser.
        /// </summary>
        /// <remarks>
        /// To avoid the Designer trying to load CefSharp.Core.Runtime we explicitly
        /// ask for NoInlining.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeBrowser(int width, int height)
        {
            NativeMethodWrapper.SetWindowPosition(BrowserHwnd, 0, 0, width, height);
        }

        /// <summary>
        /// When minimized set the browser window size to 0x0 to reduce resource usage.
        /// https://github.com/chromiumembedded/cef/blob/c7701b8a6168f105f2c2d6b239ce3958da3e3f13/tests/cefclient/browser/browser_window_std_win.cc#L87
        /// </summary>
        internal virtual void HideInternal()
        {
            NativeMethodWrapper.SetWindowPosition(BrowserHwnd, 0, 0, 0, 0);
        }

        /// <summary>
        /// Show the browser (called after previous minimised)
        /// </summary>
        internal virtual void ShowInternal()
        {
            NativeMethodWrapper.SetWindowPosition(BrowserHwnd, 0, 0, Width, Height);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                BrowserHwnd = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }
    }
}
