// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Wpf.Example.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CefSharp.Wpf.Example.Handlers
{
    public class DisplayHandler : CefSharp.Handler.DisplayHandler
    {
        private Grid parent;
        private Window fullScreenWindow;

        protected override void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (fullscreen)
                {
                    //In this example the parent is a Grid, if your parent is a different type
                    //of control then update this code accordingly.
                    parent = (Grid)VisualTreeHelper.GetParent(webBrowser);

                    //NOTE: If the ChromiumWebBrowser instance doesn't have a direct reference to
                    //the DataContext in this case the BrowserTabViewModel then your bindings won't
                    //be updated/might cause issues like the browser reloads the Url when exiting
                    //fullscreen.
                    parent.Children.Remove(webBrowser);

                    fullScreenWindow = new Window
                    {
                        WindowStyle = WindowStyle.None,
                        WindowState = WindowState.Maximized,
                        Content = webBrowser                        
                    };

                    fullScreenWindow.ShowDialog();
                }
                else
                {
                    fullScreenWindow.Content = null;

                    parent.Children.Add(webBrowser);

                    fullScreenWindow.Close();
                    fullScreenWindow = null;
                    parent = null;
                }
            }));
        }
    }
}
