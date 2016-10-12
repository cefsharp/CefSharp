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
            //Set newBrowser to null unless your attempting to host the popup in a new instance of ChromiumWebBrowser
            newBrowser = null;

            return false; //Return true to cancel the popup creation

            //EXPERIMENTAL OPTION #1: Demonstrates using a new instance of ChromiumWebBrowser to host the popup.
            //var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            //ChromiumWebBrowser chromiumBrowser = null;

            //var windowX = windowInfo.X;
            //var windowY = windowInfo.Y;
            //var windowWidth = (windowInfo.Width == int.MinValue) ? 600 : windowInfo.Width;
            //var windowHeight = (windowInfo.Height == int.MinValue) ? 800 : windowInfo.Height;

            //chromiumWebBrowser.Invoke(new Action(() =>
            //{
            //    var owner = chromiumWebBrowser.FindForm();
            //    chromiumBrowser = new ChromiumWebBrowser(targetUrl)
            //    {
            //        LifeSpanHandler = this
            //    };

            //    //NOTE: This is important and must be called before the handle is created
            //    chromiumBrowser.SetAsPopup();

            //    //Ask nicely for the control to create it's underlying handle as we'll need to
            //    //pass it to IWindowInfo.SetAsChild
            //    chromiumBrowser.CreateControl();

            //    var popup = new Form
            //    {
            //        Left = windowX,
            //        Top = windowY,
            //        Width = windowWidth,
            //        Height = windowHeight,
            //        Text = targetFrameName
            //    };

            //    owner.AddOwnedForm(popup);

            //    popup.Controls.Add(new Label { Text = "CefSharp Custom Popup" });
            //    popup.Controls.Add(chromiumBrowser);

            //    popup.Show();

            //    var rect = chromiumBrowser.ClientRectangle;

            //    //This is key, need to tell the Browser which handle will it's parent
            //    //You maybe able to pass in 0 values for left, top, right and bottom though it's safest to provide them
            //    windowInfo.SetAsChild(chromiumBrowser.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
            //}));

            //newBrowser = chromiumBrowser;

            //return false;

            //EXPERIMENTAL OPTION #2: Use IWindowInfo.SetAsChild to specify the parent handle
            //NOTE: Window resize not yet handled - it should be possible to get the
            // IBrowserHost from the newly created IBrowser instance that represents the popup
            // Then subscribe to window resize notifications and call NotifyMoveOrResizeStarted
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
