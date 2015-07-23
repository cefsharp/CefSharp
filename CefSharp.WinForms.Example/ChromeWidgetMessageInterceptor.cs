// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    /// <summary>
    /// Intercepts Windows messages sent to the ChromiumWebBrowser control's widget sub-window.
    /// 
    /// It is necessary to listen to the widget sub-window because it receives all Windows messages
    /// and forwards them to CEF, rather than the ChromiumWebBrowser.Handle.
    /// 
    /// When the widget receives a WM_MOUSEACTIVATE message, this sends a WM_NCLBUTTONDOWN message
    /// to the parent form.  This is in turn processed by any active ToolStrip or ContextMenuStrip
    /// on the form, which will close itself.
    /// </summary>
    class ChromeWidgetMessageInterceptor : NativeWindow
    {
        private ChromeWidgetMessageInterceptor(ChromiumWebBrowser browser, IntPtr chromeWidgetHostHandle)
        {
            AssignHandle(chromeWidgetHostHandle);

            browser.HandleDestroyed += BrowserHandleDestroyed;
        }

        /// <summary>
        /// Asynchronously wait for the Chromium widget window to be created for the given ChromiumWebBrowser,
        /// and fire the onCreated action on the WinForms UI thread when it exists.
        /// </summary>
        /// <param name="browser">The browser to intercept Windows messages for.</param>
        /// <param name="onCreated">This callback is fired when Chromium's widget window is created.  You should
        /// keep a reference to the supplied ChromeWidgetMessageInterceptor to ensure it is not garbage collected
        /// until your Form is disposed.</param>
        internal static void SetupLoop(ChromiumWebBrowser browser, Action<ChromeWidgetMessageInterceptor> onCreated)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    bool foundWidget = false;
                    while (!foundWidget)
                    {
                        browser.Invoke((Action)(() =>
                        {
                            IntPtr chromeWidgetHostHandle;
                            if (ChromeWidgetHandleFinder.TryFindHandle(browser, out chromeWidgetHostHandle))
                            {
                                foundWidget = true;
                                onCreated(new ChromeWidgetMessageInterceptor(browser, chromeWidgetHostHandle));
                            }
                            else
                            {
                                // Chrome hasn't yet set up its message-loop window.
                                Thread.Sleep(10);
                            }
                        }));
                    }
                }
                catch
                {
                    // Errors are likely to occur if browser is disposed, and no good way to check from another thread
                }
            });
        }

        private void BrowserHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        const int WM_MOUSEACTIVATE = 0x0021;
        const int WM_NCLBUTTONDOWN = 0x00A1;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_MOUSEACTIVATE)
            {
                // The default processing of WM_MOUSEACTIVATE results in MA_NOACTIVATE,
                // and the subsequent mouse click is eaten by Chrome.
                // This means any .NET ToolStrip or ContextMenuStrip does not get closed.
                // By posting a WM_NCLBUTTONDOWN message to a harmless co-ordinate of the
                // top-level window, we rely on the ToolStripManager's message handling
                // to close any open dropdowns:
                // http://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/ToolStripManager.cs,1249
                var topLevelWindowHandle = m.WParam;
                IntPtr lParam = IntPtr.Zero;
                PostMessage(topLevelWindowHandle, WM_NCLBUTTONDOWN, IntPtr.Zero, lParam);
            }
        }
    }

    class ChromeWidgetHandleFinder
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private readonly IntPtr _MainHandle;

        string seekClassName;
        IntPtr descendantFound;

        private ChromeWidgetHandleFinder(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        private IntPtr FindDescendantByClassName(string className)
        {
            descendantFound = IntPtr.Zero;
            seekClassName = className;

            EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
            EnumChildWindows(this._MainHandle, childProc, IntPtr.Zero);

            return descendantFound;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buffer = new StringBuilder(128);
            GetClassName(hWnd, buffer, buffer.Capacity);

            if (buffer.ToString() == seekClassName)
            {
                descendantFound = hWnd;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Chrome's message-loop Window isn't created synchronously, so this may not find it.
        /// If so, you need to wait and try again later.
        /// </summary>
        public static bool TryFindHandle(ChromiumWebBrowser browser, out IntPtr chromeWidgetHostHandle)
        {
            var browserHandle = browser.Handle;
            var windowHandleInfo = new ChromeWidgetHandleFinder(browserHandle);
            const string chromeWidgetHostClassName = "Chrome_RenderWidgetHostHWND";
            chromeWidgetHostHandle = windowHandleInfo.FindDescendantByClassName(chromeWidgetHostClassName);
            return chromeWidgetHostHandle != IntPtr.Zero;
        }
    }
}
