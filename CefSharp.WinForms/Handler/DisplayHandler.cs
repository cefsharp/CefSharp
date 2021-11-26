// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.WinForms.Host;

namespace CefSharp.WinForms.Handler
{
    /// <summary>
    /// A WinForms Specific <see cref="IDisplayHandler"/> implementation that simplifies
    /// the process of hosting a Popup as a Control/Tab. Use this implementation as a base
    /// for your own custom implementation if you are using <see cref="LifeSpanHandler.Create"/>
    /// </summary>
    public class DisplayHandler : CefSharp.Handler.DisplayHandler
    {
        /// <inheritdoc />
        protected override void OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs args)
        {
            var browser = args.Browser;

            if(browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnAddressChanged(args);
            }            

            base.OnAddressChanged(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnConsoleMessage(args);
            }

            return base.OnConsoleMessage(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override void OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnTitleChanged(args);
            }

            base.OnTitleChanged(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override void OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnStatusMessage(args);
            }

            base.OnStatusMessage(chromiumWebBrowser, args);
        }
    }
}
