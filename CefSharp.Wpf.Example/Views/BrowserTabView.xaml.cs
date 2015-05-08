// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Handlers;
using System.Windows;

namespace CefSharp.Wpf.Example.Views
{
    public partial class BrowserTabView : UserControl
    {
        public BrowserTabView()
        {
            InitializeComponent();

            browser.RequestHandler = new RequestHandler();
            browser.RegisterJsObject("bound", new BoundObject());

            browser.Loaded += browser_Loaded;
            browser.MenuHandler = new Handlers.MenuHandler();
            browser.GeolocationHandler = new Handlers.GeolocationHandler();
            browser.DownloadHandler = new DownloadHandler();
            browser.PreviewTextInput += (o, e) =>
            {
                foreach (var character in e.Text)
                {
                    browser.SendKeyEvent((int)WM.CHAR, character, 0);
                }

                e.Handled = true;
            };

            CefExample.RegisterTestResources(browser);
        }

        void browser_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            browser.LifeSpanHandler = new LifespanHandler(Window.GetWindow(this));
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void OnTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

    }
}
