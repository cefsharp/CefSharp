// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CefSharp.WinForms.Example.Helper;

namespace CefSharp.WinForms.Example.Handlers
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        private Dictionary<int, PopupAsChildHelper> popupasChildHelpers = new Dictionary<int, PopupAsChildHelper>();

        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //Set newBrowser to null unless your attempting to host the popup in a new instance of ChromiumWebBrowser
            //This option is typically used in WPF. This example demos using IWindowInfo.SetAsChild
            //Older branches likely still have an example of this method if you choose to go down that path.
            newBrowser = null;

            //Use IWindowInfo.SetAsChild to specify the parent handle
            //NOTE: user PopupAsChildHelper to handle with Form move and Control resize
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Invoke(new Action(() =>
            {
                if (chromiumWebBrowser.FindForm() is BrowserForm owner)
                {
                    var control = new Control
                    {
                        Dock = DockStyle.Fill
                    };
                    control.CreateControl();

                    owner.AddTab(control, targetUrl);

                    var rect = control.ClientRectangle;

                    windowInfo.SetAsChild(control.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
                }
            }));

            return false;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                var windowHandle = browser.GetHost().GetWindowHandle();

                //WinForms will kindly lookup the child control from it's handle
                //If no parentControl then likely it's a popup and has no parent handle
                //(Devtools by default will remain a popup, at this point the Url hasn't been set, so 
                // we're going with this assumption as it fits the use case of this example)
                var parentControl = Control.FromChildHandle(windowHandle);

                if (parentControl != null)
                {
                    var interceptor = new PopupAsChildHelper(browser);

                    popupasChildHelpers.Add(browser.Identifier, interceptor);
                }
            }
        }

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            //The default CEF behaviour (return false) will send a OS close notification (e.g. WM_CLOSE).
            //See the doc for this method for full details.    
            // Allow devtools to close
            if (browser.MainFrame.Url.Equals("chrome-devtools://devtools/devtools_app.html"))
            {
                return false;
            }

            var windowHandle = browser.GetHost().GetWindowHandle();

            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            //If browser is disposed or the handle has been released then we don't
            //need to remove the tab (likely removed from menu)
            if (!chromiumWebBrowser.IsDisposed && chromiumWebBrowser.IsHandleCreated)
            {
                chromiumWebBrowser.Invoke(new Action(() =>
                {
                    if (chromiumWebBrowser.FindForm() is BrowserForm owner)
                    {
                        owner.RemoveTab(windowHandle);
                    }
                }));
            }

            //The default CEF behaviour (return false) will send a OS close notification (e.g. WM_CLOSE).
            //See the doc for this method for full details.
            //return true here to handle closing yourself (no WM_CLOSE will be sent).
            return true;
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                if (popupasChildHelpers.TryGetValue(browser.Identifier, out PopupAsChildHelper interceptor))
                {
                    popupasChildHelpers[browser.Identifier] = null;
                    interceptor.Dispose();
                }
            }
        }
    }
}
