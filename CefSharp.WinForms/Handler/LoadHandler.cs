// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.WinForms.Host;

namespace CefSharp.WinForms.Handler
{
    /// <summary>
    /// A WinForms Specific <see cref="ILoadHandler"/> implementation that simplifies
    /// the process of hosting a Popup as a Control/Tab.Use this implementation as a base
    /// for your own custom implementation if you are using <see cref="LifeSpanHandler.Create"/>
    /// </summary>
    public class LoadHandler : CefSharp.Handler.LoadHandler
    {
        /// <inheritdoc />
        protected override void OnFrameLoadEnd(IWebBrowser chromiumWebBrowser, FrameLoadEndEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnFrameLoadEnd(args);
            }

            base.OnFrameLoadEnd(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override void OnFrameLoadStart(IWebBrowser chromiumWebBrowser, FrameLoadStartEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnFrameLoadStart(args);
            }

            base.OnFrameLoadStart(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override void OnLoadError(IWebBrowser chromiumWebBrowser, LoadErrorEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnLoadError(args);
            }

            base.OnLoadError(chromiumWebBrowser, args);
        }

        /// <inheritdoc />
        protected override void OnLoadingStateChange(IWebBrowser chromiumWebBrowser, LoadingStateChangedEventArgs args)
        {
            var browser = args.Browser;

            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                control?.OnLoadingStateChange(args);
            }

            base.OnLoadingStateChange(chromiumWebBrowser, args);
        }
    }
}
