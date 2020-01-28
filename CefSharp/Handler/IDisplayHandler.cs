// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

using Size = CefSharp.Structs.Size;

namespace CefSharp
{
    /// <summary>
    /// Handle events related to browser display state.
    /// </summary>
    public interface IDisplayHandler
    {
        /// <summary>
        /// Called when a frame's address has changed. 
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="addressChangedArgs">args</param>
        void OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs);

        /// <summary>
        /// Called when auto-resize is enabled via IBrowserHost.SetAutoResizeEnabled and the contents have auto-resized.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="newSize">will be the desired size in view coordinates</param>
        /// <returns>Return true if the resize was handled or false for default handling. </returns>
        bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize);

        /// <summary>
        /// Called when the page title changes.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="titleChangedArgs">args</param>
        void OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs);

        /// <summary>
        /// Called when the page icon changes.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="urls">list of urls where the favicons can be downloaded</param>
        void OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls);

        /// <summary>
        /// Called when web content in the page has toggled fullscreen mode. The client is
        /// responsible for resizing the browser if desired.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="fullscreen">If true the content will automatically be sized to fill the browser content area.
        /// If false the content will automatically return to its original size and position.</param>
        void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen);

        /// <summary>
        /// Called when the overall page loading progress has changed
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="progress">ranges from 0.0 to 1.0.</param>
        void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress);

        /// <summary>
        /// Called when the browser is about to display a tooltip. text contains the
        /// text that will be displayed in the tooltip. You can optionally modify text
        /// and then return false to allow the browser to display the tooltip.
        /// When window rendering is disabled the application is responsible for
        /// drawing tooltips and the return value is ignored.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="text">the text that will be displayed in the tooltip</param>
        /// <returns>To handle the display of the tooltip yourself return true otherwise return false
        /// to allow the browser to display the tooltip.</returns>
        /// <remarks>Only called when using Off-screen rendering (WPF and OffScreen)</remarks>
        bool OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text);

        /// <summary>
        /// Called when the browser receives a status message.
        /// </summary>
        /// <param name="chromiumWebBrowser">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="statusMessageArgs">args</param>
        void OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs);

        /// <summary>
        /// Called to display a console message. 
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="consoleMessageArgs">args</param>
        /// <returns>Return true to stop the message from being output to the console.</returns>
        bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs);
    }
}
