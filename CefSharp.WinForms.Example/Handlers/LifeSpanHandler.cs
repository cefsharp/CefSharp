// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
            newBrowser = null;

            //Use IWindowInfo.SetAsChild to specify the parent handle
            //NOTE: user PopupAsChildHelper to handle with Form move and Control resize
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Invoke(new Action(() =>
            {
                var owner = chromiumWebBrowser.FindForm() as BrowserForm;

                if(owner != null)
                {
                    var control = new Control();
                    control.Dock = DockStyle.Fill;
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
                var interceptor = new PopupAsChildHelper(browser);

                popupasChildHelpers.Add(browser.Identifier, interceptor);
            }
        }

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            //We need to allow popups to close
            //If the browser has been disposed then we'll just let the default behaviour take place
            if (browser.IsDisposed || browser.IsPopup)
            {
                return false;
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
                var interceptor = popupasChildHelpers[browser.Identifier];

                if (interceptor != null)
                {
                    popupasChildHelpers[browser.Identifier] = null;
                    interceptor.Dispose();
                }
            }
        }
    }
}
