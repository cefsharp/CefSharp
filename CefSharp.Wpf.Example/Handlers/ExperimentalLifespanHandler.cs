// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CefSharp.Wpf.Example.Handlers
{
    /// <summary>
    /// WPF - EXPERIMENTAL LifeSpanHandler implementation that demos hosting a popup in a new ChromiumWebBrowser instance
    /// hosted in a new Window.
    /// </summary>
    public class ExperimentalLifespanHandler : CefSharp.Handler.LifeSpanHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        private static string GetWindowTitle(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            var sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        protected override bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            ChromiumWebBrowser popupChromiumWebBrowser = null;

            var windowX = (windowInfo.X == int.MinValue) ? double.NaN : windowInfo.X;
            var windowY = (windowInfo.Y == int.MinValue) ? double.NaN : windowInfo.Y;
            var windowWidth = (windowInfo.Width == int.MinValue) ? double.NaN : windowInfo.Width;
            var windowHeight = (windowInfo.Height == int.MinValue) ? double.NaN : windowInfo.Height;

            // Invoke onto the WPF UI Thread to create a new Window/UI Control
            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = System.Windows.Window.GetWindow(chromiumWebBrowser);
                popupChromiumWebBrowser = new ChromiumWebBrowser();

                popupChromiumWebBrowser.SetAsPopup();
                popupChromiumWebBrowser.LifeSpanHandler = this;

                var popup = new System.Windows.Window
                {
                    Left = windowX,
                    Top = windowY,
                    Width = windowWidth,
                    Height = windowHeight,
                    Content = popupChromiumWebBrowser,
                    Owner = owner,
                    Title = targetFrameName
                };

                var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(popup);
                //Create the handle Window handle (In WPF there's only one handle per window, not per control)
                var handle = windowInteropHelper.EnsureHandle();

                //The parentHandle value will be used to identify monitor info and to act as the parent window for dialogs,
                //context menus, etc. If parentHandle is not provided then the main screen monitor will be used and some
                //functionality that requires a parent window may not function correctly.
                windowInfo.SetAsWindowless(handle);

                popup.Closed += (o, e) =>
                {
                    var w = o as System.Windows.Window;
                    if (w != null && w.Content is IWebBrowser)
                    {
                        (w.Content as IWebBrowser).Dispose();
                        w.Content = null;
                    }
                };
            });

            newBrowser = popupChromiumWebBrowser;

            return false;
        }

        protected override void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                var windowTitle = GetWindowTitle(browser.GetHost().GetWindowHandle());

                //CEF doesn't currently provide an option to determine if the new Popup is
                //DevTools so we use a hackyworkaround to check the Window Title.
                //DevTools is hosted in it's own popup, we don't perform any action here
                if (windowTitle != "DevTools")
                {
                    var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                    chromiumWebBrowser.Dispatcher.Invoke(() =>
                    {
                        var owner = System.Windows.Window.GetWindow(chromiumWebBrowser);

                        if (owner != null && owner.Content == browserControl)
                        {
                            owner.Show();
                        }
                    });
                }
            }
        }

        protected override void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                //DevTools is hosted in it's own popup, we don't perform any action here
                if (!browser.MainFrame.Url.Equals("devtools://devtools/devtools_app.html"))
                {
                    var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                    chromiumWebBrowser.Dispatcher.Invoke(() =>
                    {
                        var owner = System.Windows.Window.GetWindow(chromiumWebBrowser);

                        if (owner != null && owner.Content == browserControl)
                        {
                            owner.Close();
                        }
                    });
                }
            }
        }
    }
}
