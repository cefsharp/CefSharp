// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Handlers
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //Default behaviour
            newBrowser = null;

            return false; //Return true to cancel the popup creation

            //EXPERIMENTAL: Demonstrates using a new instance of ChromiumWebBrowser to host the popup.
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            ChromiumWebBrowser chromiumBrowser = null;

            var windowX = windowInfo.X;
            var windowY = windowInfo.Y;
            var windowWidth = (windowInfo.Width == int.MinValue) ? 600 : windowInfo.Width;
            var windowHeight = (windowInfo.Height == int.MinValue) ? 800 : windowInfo.Height;

            chromiumWebBrowser.Invoke(new Action(() =>
            {
                var owner = chromiumWebBrowser.FindForm();
                chromiumBrowser = new ChromiumWebBrowser(targetUrl)
                {
                    LifeSpanHandler = this
                };
                chromiumBrowser.SetAsPopup();

                var popup = new Form
                {
                    Left = windowX,
                    Top = windowY,
                    Width = windowWidth,
                    Height = windowHeight,
                    Text = targetFrameName
                };

                owner.AddOwnedForm(popup);

                popup.Controls.Add(new Label { Text = "CefSharp Custom Popup" });
                popup.Controls.Add(chromiumBrowser);

                popup.Show();
            }));

            newBrowser = chromiumBrowser;

            return false;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            
        }

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            
        }
    }
}
