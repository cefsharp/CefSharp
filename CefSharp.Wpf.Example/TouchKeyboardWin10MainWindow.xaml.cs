// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using CefSharp.Enums;
using Microsoft.Windows.Input.TouchKeyboard;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// TouchKeyboardWin10MainWindow provides a very basic Windows 10 only example of
    /// showing the onscreen(virtual) keyboard in a WPF app.
    /// </summary>
    public partial class TouchKeyboardWin10MainWindow : Window
    {
        private TouchKeyboardEventManager touchKeyboardEventManager;

        public TouchKeyboardWin10MainWindow()
        {
            InitializeComponent();

            Browser.VirtualKeyboardRequested += BrowserVirtualKeyboardRequested;
            Browser.IsBrowserInitializedChanged += BrowserIsBrowserInitializedChanged;
        }

        private void BrowserIsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var browserHost = Browser.GetBrowserHost();

                touchKeyboardEventManager = new TouchKeyboardEventManager(browserHost.GetWindowHandle());
            }
            else
            {
                if (touchKeyboardEventManager != null)
                {
                    touchKeyboardEventManager.Dispose();
                }
            }
        }

        private void BrowserVirtualKeyboardRequested(object sender, VirtualKeyboardRequestedEventArgs e)
        {
            var inputPane = touchKeyboardEventManager.GetInputPane();

            if (e.TextInputMode == TextInputMode.None)
            {
                inputPane.TryHide();
            }
            else
            {
                inputPane.TryShow();
            }
        }
    }
}
