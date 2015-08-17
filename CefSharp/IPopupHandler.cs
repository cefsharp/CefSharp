// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Handler methods that get called AFTER the Popup is created.
    /// </summary>
    public interface IPopupHandler
    {
        /// <summary>
        /// Called after the paased popup window (browser) is created.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="browser">The popup window instance that was just created.</param>
        void OnAfterCreated(IWebBrowser browserControl, IBrowser browser);

        /// <summary>
        /// Called before the passed popup window (browser) is closed.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="browser">The popup window instance that is closing.</param>
        void OnBeforeClose(IWebBrowser browserControl, IBrowser browser);

        /// <summary>
        /// Called when a frame's address has changed. 
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="addressChangedArgs">args</param>
        void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs);

        /// <summary>
        /// Called when the loading state has changed. This callback will be executed twice
        /// once when loading is initiated either programmatically or by user action,
        /// and once when loading is terminated due to completion, cancellation of failure. 
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="loadingStateChangedArgs">args</param>
        void OnLoadingStateChange(IWebBrowser browserControl, LoadingStateChangedEventArgs loadingStateChangedArgs);

        /// <summary>
        /// Called when the browser receives a status message.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this popup is related to.</param>
        /// <param name="statusMessageArgs">args</param>
        void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs);

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
