// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.WinForms.Host;

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
        /// <param name="chromiumWebBrowser">the <see cref="ChromiumWebBrowser"/> or <see cref="ChromiumHostControl"/> instance.</param>
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
        public static bool DestroyWindow(this IChromiumWebBrowserBase chromiumWebBrowser)
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

            var browser = chromiumWebBrowser.BrowserCore;

            if (browser == null)
            {
                return false;
            }

            var handle = browser.GetHost().GetWindowHandle();


            return DestroyWindow(handle);
        }

        /// <summary>
        /// Open DevTools using <paramref name="parentControl"/> as the parent control. If inspectElementAtX and/or inspectElementAtY are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// For resize/moving to work correctly you will need to use the <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/> implementation.
        /// (Set <see cref="ChromiumWebBrowser.LifeSpanHandler"/> to an instance of <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/>)
        /// </summary>
        /// <param name="chromiumWebBrowser"><see cref="ChromiumWebBrowser"/> instance</param>
        /// <param name="parentControl">Control used as the parent for DevTools (a custom control will be added to the <see cref="Control.Controls"/> collection)</param>
        /// <param name="controlName">Control name</param>
        /// <param name="dockStyle">Dock Style</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        /// <returns>Returns the <see cref="Control"/> that hosts the DevTools instance if successful, otherwise returns null on error.</returns>
        public static Control ShowDevToolsDocked(this IChromiumWebBrowserBase chromiumWebBrowser, Control parentControl, string controlName = nameof(ChromiumHostControl) + "DevTools", DockStyle dockStyle = DockStyle.Fill, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            if (chromiumWebBrowser.IsDisposed || parentControl == null || parentControl.IsDisposed)
            {
                return null;
            }

            return chromiumWebBrowser.ShowDevToolsDocked((ctrl) => { parentControl.Controls.Add(ctrl); }, controlName, dockStyle, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Open DevTools using your own Control as the parent. If inspectElementAtX and/or inspectElementAtY are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// For resize/moving to work correctly you will need to use the <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/> implementation.
        /// (Set <see cref="ChromiumWebBrowser.LifeSpanHandler"/> to an instance of <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/>)
        /// </summary>
        /// <param name="chromiumWebBrowser"><see cref="ChromiumWebBrowser"/> instance</param>
        /// <param name="addParentControl">
        /// Action that is Invoked when the DevTools Host Control has been created and needs to be added to it's parent.
        /// It's important the control is added to it's intended parent at this point so the <see cref="Control.ClientRectangle"/>
        /// can be calculated to set the initial display size.</param>
        /// <param name="controlName">control name</param>
        /// <param name="dockStyle">Dock Style</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        /// <returns>Returns the <see cref="Control"/> that hosts the DevTools instance if successful, otherwise returns null on error.</returns>
        public static Control ShowDevToolsDocked(this IChromiumWebBrowserBase chromiumWebBrowser, Action<ChromiumHostControl> addParentControl, string controlName = nameof(ChromiumHostControl) + "DevTools", DockStyle dockStyle = DockStyle.Fill, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            if (chromiumWebBrowser.IsDisposed || addParentControl == null)
            {
                return null;
            }

            var host = chromiumWebBrowser.GetBrowserHost();
            if (host == null)
            {
                return null;
            }

            var control = new ChromiumHostControl()
            {
                Name = controlName,
                Dock = dockStyle
            };

            control.CreateControl();

            //It's now time for the user to add the control to it's parent
            addParentControl(control);

            //Devtools will be a child of the ChromiumHostControl
            var rect = control.ClientRectangle;
            var windowInfo = new WindowInfo();
            var windowBounds = new CefSharp.Structs.Rect(rect.X, rect.Y, rect.Width, rect.Height);
            windowInfo.SetAsChild(control.Handle, windowBounds);
            host.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);

            return control;
        }
    }
}
