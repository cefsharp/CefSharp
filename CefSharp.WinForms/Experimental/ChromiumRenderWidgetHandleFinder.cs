// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CefSharp.WinForms.Experimental
{
    /// <summary>
    /// ChromiumWidgetHandleFinder is a helper class used to find the <see cref="ChromeRenderWidgetHostClassName"/>
    /// child Hwnd for the browser instance.
    /// </summary>
    public static class ChromiumRenderWidgetHandleFinder
    {
        /// <summary>
        /// Class Name of the Chrome_RenderWidgetHostHWND Child Window
        /// </summary>
        public const string ChromeRenderWidgetHostClassName = "Chrome_RenderWidgetHostHWND";

        /// <summary>
        /// EnumWindowProc delegate used by <see cref="EnumChildWindows(IntPtr, EnumWindowProc, IntPtr)"/>
        /// </summary>
        /// <param name="hwnd">A handle to a child window of the parent window specified in EnumChildWindows</param>
        /// <param name="lParam">The application-defined value given in EnumChildWindows</param>
        /// <returns>To continue enumeration, the callback function must return true; to stop enumeration, it must return false.</returns>
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// Chromium's message-loop Window isn't created synchronously, so this may not find it.
        /// If so, you need to wait and try again later.
        /// </summary>
        /// <param name="chromiumWebBrowser">ChromiumWebBrowser instance</param>
        /// <param name="chromerRenderWidgetHostHandle">Handle of the child HWND with the name <see cref="ChromeRenderWidgetHostClassName"/></param>
        /// <returns>returns true if the HWND was found otherwise false.</returns>
        public static bool TryFindHandle(IWebBrowser chromiumWebBrowser, out IntPtr chromerRenderWidgetHostHandle)
        {
            var host = chromiumWebBrowser.GetBrowserHost();
            if (host == null)
            {
                throw new Exception("IBrowserHost is null, you've likely call this method before the underlying browser has been created.");
            }

            var hwnd = host.GetWindowHandle();

            return TryFindHandle(hwnd, ChromeRenderWidgetHostClassName, out chromerRenderWidgetHostHandle);
        }

        /// <summary>
        /// Chromium's message-loop Window isn't created synchronously, so this may not find it.
        /// If so, you need to wait and try again later.
        /// </summary>
        /// <param name="browser">IBrowser instance</param>
        /// <param name="chromerRenderWidgetHostHandle">Handle of the child HWND with the name <see cref="ChromeRenderWidgetHostClassName"/></param>
        /// <returns>returns true if the HWND was found otherwise false.</returns>
        public static bool TryFindHandle(IBrowser browser, out IntPtr chromerRenderWidgetHostHandle)
        {
            var host = browser.GetHost();
            if (host == null)
            {
                throw new Exception("IBrowserHost is null, you've likely call this method before the underlying browser has been created.");
            }

            var hwnd = host.GetWindowHandle();

            return TryFindHandle(hwnd, ChromeRenderWidgetHostClassName, out chromerRenderWidgetHostHandle);
        }

        /// <summary>
        /// Helper function used to find the child HWND with the ClassName matching <paramref name="chromeRenderWidgetHostClassName"/>
        /// Chromium's message-loop Window isn't created synchronously, so this may not find it.
        /// If so, you need to wait and try again later.
        /// In most cases you should use the <see cref="TryFindHandle(IWebBrowser, out IntPtr)"/> overload.
        /// </summary>
        /// <param name="chromiumWebBrowserHandle"><see cref="ChromiumWebBrowser"/> control Handle</param>
        /// <param name="chromeRenderWidgetHostClassName">class name used to match</param>
        /// <param name="chromerRenderWidgetHostHandle">Handle of the child HWND with the name <see cref="ChromeRenderWidgetHostClassName"/></param>
        /// <returns>returns true if the HWND was found otherwise false.</returns>
        public static bool TryFindHandle(IntPtr chromiumWebBrowserHandle, string chromeRenderWidgetHostClassName, out IntPtr chromerRenderWidgetHostHandle)
        {
            var chromeRenderWidgetHostHwnd = IntPtr.Zero;

            EnumWindowProc childProc = (IntPtr hWnd, IntPtr lParam) =>
            {
                var buffer = new StringBuilder(128);
                GetClassName(hWnd, buffer, buffer.Capacity);

                if (buffer.ToString() == chromeRenderWidgetHostClassName)
                {
                    chromeRenderWidgetHostHwnd = hWnd;
                    return false;
                }

                return true;
            };

            EnumChildWindows(chromiumWebBrowserHandle, childProc, IntPtr.Zero);

            chromerRenderWidgetHostHandle = chromeRenderWidgetHostHwnd;

            return chromerRenderWidgetHostHandle != IntPtr.Zero;
        }
    }
}

