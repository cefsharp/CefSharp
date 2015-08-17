// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
        /// Called when the browser is about to display a tooltip. |text| contains the
        /// text that will be displayed in the tooltip. To handle the display of the
        /// tooltip yourself return true. Otherwise, you can optionally modify |text|
        /// and then return false to allow the browser to display the tooltip.
        /// When window rendering is disabled the application is responsible for
        /// drawing tooltips and the return value is ignored.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="text"></param>
        void OnTooltip(IWebBrowser browserControl, string text);

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
        /// <param name="consoleMessageArgs"></param>
        void OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs);

        /// <summary>
        /// Called when the loading state has changed. This callback will be executed twice
        /// once when loading is initiated either programmatically or by user action,
        /// and once when loading is terminated due to completion, cancellation of failure. 
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="loadingStateChangedArgs">args</param>
        void OnLoadingStateChange(IWebBrowser browserControl, LoadingStateChangedEventArgs loadingStateChangedArgs);

        /// <summary>
        /// Called when the browser begins loading a frame.
        /// The |<see cref="FrameLoadEndEventArgs.Frame"/> value will never be empty
        /// Check the <see cref="IFrame.IsMain"/> method to see if this frame is the main frame.
        /// Multiple frames may be loading at the same time. Sub-frames may start or continue loading after the main frame load has ended.
        /// This method may not be called for a particular frame if the load request for that frame fails.
        /// For notification of overall browser load status use <see cref="OnLoadingStateChange"/> instead. 
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="frameLoadStartArgs">args</param>
        void OnFrameLoadStart(IWebBrowser browserControl, FrameLoadStartEventArgs frameLoadStartArgs);

        /// <summary>
        /// Called when the browser is done loading a frame.
        /// The |<see cref="FrameLoadEndEventArgs.Frame"/> value will never be empty
        /// Check the <see cref="IFrame.IsMain"/> method to see if this frame is the main frame.
        /// Multiple frames may be loading at the same time. Sub-frames may start or continue loading after the main frame load has ended.
        /// This method will always be called for all frames irrespective of whether the request completes successfully. 
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="frameLoadEndArgs">args</param>
        void OnFrameLoadEnd(IWebBrowser browserControl, FrameLoadEndEventArgs frameLoadEndArgs);

        /// <summary>
        /// Called when the resource load for a navigation fails or is canceled.
        /// |errorCode| is the error code number, |errorText| is the error text and
        /// |failedUrl| is the URL that failed to load. See net\base\net_error_list.h
        /// for complete descriptions of the error codes.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="loadErrorArgs">args</param>
        void OnLoadError(IWebBrowser browserControl, LoadErrorEventArgs loadErrorArgs);
    }
}
