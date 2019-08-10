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
        private bool handlePopups;
        private Dictionary<int, PopupAsChildHelper> popupasChildHelpers = new Dictionary<int, PopupAsChildHelper>();

        public LifeSpanHandler(bool handleBrowserPopups)
        {
            handlePopups = handleBrowserPopups;
        }

        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //NOTE: DevTools popups DO NOT trigger OnBeforePopup.

            //Set newBrowser to null unless your attempting to host the popup in a new instance of ChromiumWebBrowser
            //This option is typically used in WPF. This example demos using IWindowInfo.SetAsChild
            //Older branches likely still have an example of this method if you choose to go down that path.
            newBrowser = null;

            //If handling popups is set to false, CEF default behaviour is used instead.
            if (handlePopups)
            {
                //Use IWindowInfo.SetAsChild to specify the parent handle
                //NOTE: user PopupAsChildHelper to handle with Form move and Control resize
                var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

                webBrowser.Invoke(new Action(() =>
                {
                    if (webBrowser.FindForm() is BrowserForm owner)
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

            }

            return false;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
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

        bool ILifeSpanHandler.DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            var windowHandle = browser.GetHost().GetWindowHandle();
            var parentControl = Control.FromChildHandle(windowHandle);
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            //The default CEF behaviour (return false) will send a OS close notification (e.g. WM_CLOSE).
            //See the doc for this method for full details.    
            // Allow devtools to close
            if (browser.MainFrame.Url.Equals("devtools://devtools/devtools_app.html"))
            {
                if (parentControl != null)
                {
                    //IBrowserHost.CloseDevTools() will not release the DevTools window handle,
                    //be mindful it will trigger the ILifeSpanHandler.DoClose() which then needs to be handled appropriately if this scenario occurs.
                    //Dispose of the DevTools parent, this will release the DevTools window handle
                    //and the ILifeSpanHandler.OnBeforeClose() will call after.
                    webBrowser.Invoke(new Action(() =>
                    {
                        parentControl.Dispose();
                    }));

                    return true;
                }

                return false;
            }

            if (handlePopups)
            {
                //If browser is disposed or the handle has been released then we don't
                //need to remove the tab (likely removed from menu)
                if (!webBrowser.IsDisposed && webBrowser.IsHandleCreated)
                {
                    webBrowser.Invoke(new Action(() =>
                    {
                        if (webBrowser.FindForm() is BrowserForm owner)
                        {
                            owner.RemoveTab(windowHandle);
                        }
                    }));
                }

                //return true here to handle closing yourself (no WM_CLOSE will be sent).
                return true;
            }

            //The default CEF behaviour (return false) will send a OS close notification (e.g. WM_CLOSE).
            //See the doc for this method for full details.
            return false;
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
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
