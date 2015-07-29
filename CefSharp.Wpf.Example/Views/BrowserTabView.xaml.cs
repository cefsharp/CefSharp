﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Handlers;

namespace CefSharp.Wpf.Example.Views
{
    public partial class BrowserTabView : UserControl
    {
        public BrowserTabView()
        {
            InitializeComponent();

            browser.RequestHandler = new RequestHandler();
            if (CefSharpSettings.WcfEnabled)
            {
                browser.RegisterJsObject("bound", new BoundObject());
            }
            browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundObject());

            browser.LifeSpanHandler = new LifespanHandler();
            browser.MenuHandler = new MenuHandler();
            browser.GeolocationHandler = new GeolocationHandler();
            browser.DownloadHandler = new DownloadHandler();
            browser.PreviewTextInput += (sender, args) =>
            {
                var host = browser.GetBrowser().GetHost();
                var keyEvent = new KeyEvent();

                foreach (var character in args.Text)
                {
                    keyEvent.WindowsKeyCode = character;
                    host.SendKeyEvent(keyEvent);
                }

                args.Handled = true;
            };

            browser.LoadError += (sender, args) =>
            {
                // Don't display an error for downloaded files.
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                // Don't display an error for external protocols that we allow the OS to
                // handle. See OnProtocolExecution().
                //if (args.ErrorCode == CefErrorCode.UnknownUrlScheme)
                //{
                //	var url = args.Frame.Url;
                //	if (url.StartsWith("spotify:"))
                //	{
                //		return;
                //	}
                //}

                // Display a load error message.
                var errorBody = string.Format("<html><body bgcolor=\"white\"><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              args.FailedUrl, args.ErrorText, args.ErrorCode);

                args.Frame.LoadStringForUrl(errorBody, args.FailedUrl);
            };

            CefExample.RegisterTestResources(browser);
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
