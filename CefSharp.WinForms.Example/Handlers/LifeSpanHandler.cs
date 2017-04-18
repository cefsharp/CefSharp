// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
            // Set newBrowser to null unless your attempting to host the popup in a new instance of ChromiumWebBrowser
            // This should only be used in WPF/OffScreen
            newBrowser = null;

            return false; //Return true to cancel the popup creation

            // Hosting the popup in your own control/window
            // Use IWindowInfo.SetAsChild to specify the parent handle
            // NOTE: Window resize not yet handled - you need to get the
            // IBrowserHost from the newly created IBrowser instance that represents the popup
            // Then subscribe to window resize notifications and call NotifyMoveOrResizeStarted().
            // Also any chances in width/height you need to call SetWindowPos on the browsers HWND
            // Use NativeMethodWrapper.SetWindowPosition to achieve this - you can get the HWND using
            // IBrowserHost method

            //var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            //var windowX = windowInfo.X;
            //var windowY = windowInfo.Y;
            //var windowWidth = (windowInfo.Width == int.MinValue) ? 600 : windowInfo.Width;
            //var windowHeight = (windowInfo.Height == int.MinValue) ? 800 : windowInfo.Height;

            //chromiumWebBrowser.Invoke(new Action(() =>
            //{
            //    var owner = chromiumWebBrowser.FindForm();

            //    var popup = new Form
            //    {
            //        Left = windowX,
            //        Top = windowY,
            //        Width = windowWidth,
            //        Height = windowHeight,
            //        Text = targetFrameName
            //    };

            //    popup.CreateControl();

            //    owner.AddOwnedForm(popup);

            //    var control = new Control();
            //    control.Dock = DockStyle.Fill;
            //    control.CreateControl();

            //    popup.Controls.Add(control);

            //    popup.Show();

            //    var rect = control.ClientRectangle;

            //    windowInfo.SetAsChild(control.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
            //}));
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {

        }

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            //We need to allow popups to close
            //If the browser has been disposed then we'll just let the default behaviour take place
            if(browser.IsDisposed || browser.IsPopup)
            {
                return false;
            }

            //The default CEF behaviour (return false) will send a OS close notification (e.g. WM_CLOSE).
            //See the doc for this method for full details.
            //return true here to handle closing yourself (no WM_CLOSE will be sent).
            return true;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {

        }
    }
}
