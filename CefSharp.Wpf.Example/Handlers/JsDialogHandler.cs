// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    public class JsDialogHandler : CefSharp.Handler.JsDialogHandler
    {
        protected override bool OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            var b = (ChromiumWebBrowser)chromiumWebBrowser;

            b.Dispatcher.InvokeAsync(() =>
            {
                if (dialogType == CefJsDialogType.Confirm)
                {
                    var messageBoxResult = MessageBox.Show(messageText, $"A page at {originUrl} says:", MessageBoxButton.YesNo);

                    callback.Continue(messageBoxResult == MessageBoxResult.Yes);
                }
                else if(dialogType == CefJsDialogType.Alert)
                {
                    var messageBoxResult = MessageBox.Show(messageText, $"A page at {originUrl} says:", MessageBoxButton.OK);

                    callback.Continue(messageBoxResult == MessageBoxResult.OK);
                }
                else if (dialogType == CefJsDialogType.Prompt)
                {
                    var messageBoxResult = PromptDialog.Prompt(messageText, $"A page at {originUrl} says:", defaultPromptText);

                    callback.Continue(messageBoxResult.Item1, userInput: messageBoxResult.Item2);
                }
            });

            return true;
        }
    }
}
