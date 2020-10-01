// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

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
    }
}
