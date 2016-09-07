﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Interop;
using CefSharp.Example;

namespace CefSharp.Wpf.Example.Handlers
{
    public class LifespanHandler : ILifeSpanHandler
    {
        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

//            return false;

            //NOTE: This is experimental
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            ChromiumWebBrowser chromiumBrowser = null;

            var windowX = (windowInfo.X == int.MinValue) ? double.NaN : windowInfo.X;
            var windowY = (windowInfo.Y == int.MinValue) ? double.NaN : windowInfo.Y;
            var windowWidth = (windowInfo.Width == int.MinValue) ? double.NaN : windowInfo.Width;
            var windowHeight = (windowInfo.Height == int.MinValue) ? double.NaN : windowInfo.Height;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);
                chromiumBrowser = new ChromiumWebBrowser
                {
                    Address = targetUrl,
                };
                chromiumBrowser.RegisterAsyncJsObject("uniqueObject", new UniqueBoundObject());

                chromiumBrowser.SetAsPopup();
                chromiumBrowser.LifeSpanHandler = this;

                var popup = new Window
                {
                    Left = windowX,
                    Top = windowY,
                    Width = windowWidth,
                    Height = windowHeight,
                    Content = chromiumBrowser,
                    Owner = owner,
                    Title = targetFrameName
                };

                var windowInteropHelper = new WindowInteropHelper(popup);
                //Create the handle Window handle (In WPF there's only one handle per window, not per control)
                var handle = windowInteropHelper.EnsureHandle();

                //The parentHandle value will be used to identify monitor info and to act as the parent window for dialogs,
                //context menus, etc. If parentHandle is not provided then the main screen monitor will be used and some
                //functionality that requires a parent window may not function correctly.
                windowInfo.SetAsWindowless(handle, true);

                popup.Closed += (o, e) =>
                {
                    var w = o as Window;
                    if (w != null && w.Content is IWebBrowser)
                    {
                        (w.Content as IWebBrowser).Dispose();
                        w.Content = null;
                    }
                };
            });

            newBrowser = chromiumBrowser;

            return false;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            //NOTE: This is experimental
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);

                if (owner != null && owner.Content == browserControl)
                {
                    owner.Show();
                }
            });
        }

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            //NOTE: This is experimental
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);

                if (owner != null && owner.Content == browserControl)
                {
                    owner.Close();
                }
            });
        }
    }
}
