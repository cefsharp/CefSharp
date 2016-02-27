// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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
    /// The supplied Action delegate is fired upon each message.
    /// </summary>
    class ChromeWidgetMessageInterceptor : NativeWindow
    {
        private readonly ChromiumWebBrowser browser;
        private Action<Message> forwardAction;

        private ChromeWidgetMessageInterceptor(ChromiumWebBrowser browser, IntPtr chromeWidgetHostHandle, Action<Message> forwardAction)
        {
            AssignHandle(chromeWidgetHostHandle);

            this.browser = browser;
            browser.HandleDestroyed += BrowserHandleDestroyed;

            this.forwardAction = forwardAction;
        }

        /// <summary>
        /// Asynchronously wait for the Chromium widget window to be created for the given ChromiumWebBrowser,
        /// and when created hook into its Windows message loop.
        /// </summary>
        /// <param name="browser">The browser to intercept Windows messages for.</param>
        /// <param name="forwardAction">This action will be called whenever a Windows message is received.</param>
        internal static void SetupLoop(ChromiumWebBrowser browser, Action<Message> forwardAction)
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
                                new ChromeWidgetMessageInterceptor(browser, chromeWidgetHostHandle, forwardAction);
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

            browser.HandleDestroyed -= BrowserHandleDestroyed;
            forwardAction = null;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (forwardAction != null)
            {
                forwardAction(m);
            }
        }
    }

    class ChromeWidgetHandleFinder
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private readonly IntPtr mainHandle;
        private string seekClassName;
        private IntPtr descendantFound;

        private ChromeWidgetHandleFinder(IntPtr handle)
        {
            this.mainHandle = handle;
        }

        private IntPtr FindDescendantByClassName(string className)
        {
            descendantFound = IntPtr.Zero;
            seekClassName = className;

            EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
            EnumChildWindows(this.mainHandle, childProc, IntPtr.Zero);

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
