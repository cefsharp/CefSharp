﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

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
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="addressChangedArgs">args</param>
        void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs);

        /// <summary>
        /// Called when the page title changes.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="titleChangedArgs">args</param>
        void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs);

        /// <summary>
        /// Called when the page icon changes.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="urls">list of urls where the favicons can be downloaded</param>
        void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls);

        /// <summary>
        /// Called when web content in the page has toggled fullscreen mode. The client is
        /// responsible for resizing the browser if desired.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="fullscreen">If true the content will automatically be sized to fill the browser content area.
        /// If false the content will automatically return to its original size and position.</param>
        void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen);

        /// <summary>
        /// Called when the browser is about to display a tooltip. |text| contains the
        /// text that will be displayed in the tooltip. To handle the display of the
        /// tooltip yourself return true. Otherwise, you can optionally modify |text|
        /// and then return false to allow the browser to display the tooltip.
        /// When window rendering is disabled the application is responsible for
        /// drawing tooltips and the return value is ignored.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="text">the text that will be displayed in the tooltip</param>
        bool OnTooltipChanged(IWebBrowser browserControl, string text);

        /// <summary>
        /// Called when the browser receives a status message.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="statusMessageArgs">args</param>
        void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs);

        /// <summary>
        /// Called to display a console message. Return true to stop the message from
        /// being output to the console.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="consoleMessageArgs">args</param>
        bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs);
    }
}
