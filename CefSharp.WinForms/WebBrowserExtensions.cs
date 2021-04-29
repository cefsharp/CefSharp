// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms
{
    /// <summary>
    /// Helper extensions for performing common CefSharp related WinForms tasks
    /// </summary>
    public static class WebBrowserExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool DestroyWindow(IntPtr hWnd);

        /// <summary>
        /// Manually call https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-destroywindow
        /// passing in the handle returned from <see cref="IBrowserHost.GetWindowHandle"/>.
        /// This method can be used to manually close the underlying CefBrowser instance.
        /// This will avoid the WM_Close message that CEF sends by default to the top level window.
        /// (Which closes your application). This method should generally only be used in the WinForms version.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser instance</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        /// <example>
        /// <code>
        /// //Invoke on the CEF UI Thread
        /// Cef.UIThreadTaskFactory.StartNew(() =>
        /// {
        ///   var closed = chromiumWebBrowser.DestroyWindow();
        /// });
        /// </code>
        /// </example>
        public static bool DestroyWindow(this IWebBrowser chromiumWebBrowser)
        {
            if (!Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
            {
                throw new InvalidOperationException("This method can only be called on the CEF UI thread." +
                    "Use Cef.UIThreadTaskFactory to marshal your call onto the CEF UI Thread.");
            }

            if (chromiumWebBrowser.IsDisposed)
            {
                return false;
            }

            var browser = chromiumWebBrowser.GetBrowser();

            if (browser == null)
            {
                return false;
            }

            var handle = browser.GetHost().GetWindowHandle();


            return DestroyWindow(handle);
        }

        /// <summary>
        /// Open developer tools using <paramref name="parentControl"/> as the parent control. If inspectElementAtX and/or inspectElementAtY are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// </summary>
        /// <param name="chromiumWebBrowser"><see cref="ChromiumWebBrowser"/> instance</param>
        /// <param name="parentControl">Control used as the parent for DevTools (a custom control will be added to the <see cref="Control.Controls"/> collection)</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        /// <returns>Returns the <see cref="Control"/> that hosts the DevTools instance if successful, otherwise returns null on error.</returns>
        public static Control ShowDevToolsDocked(this IWebBrowser chromiumWebBrowser, Control parentControl, string controlName = nameof(ChromiumHostControl) + "DevTools", DockStyle dockStyle = DockStyle.Fill, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            if(chromiumWebBrowser.IsDisposed || parentControl == null || parentControl.IsDisposed)
            {
                return null;
            }

            var host = chromiumWebBrowser.GetBrowserHost();
            if(host == null)
            {
                return null;
            }

            var control = new ChromiumHostControl()
            {
                Name = controlName,
                Dock = dockStyle
            };

            control.CreateControl();

            parentControl.Controls.Add(control);

            //Devtools will be a child of the ChromiumHostControl
            var rect = control.ClientRectangle;
            var windowInfo = new WindowInfo();
            windowInfo.SetAsChild(control.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
            host.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);

            return control;
        }
    }
}
