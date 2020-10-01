// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to browser load status.
    /// The methods of this interface will be called on the CEF UI thread. Blocking in these methods
    /// will likely cause your UI to become unresponsive and/or hang.
    /// </summary>
    public interface ILoadHandler
    {
        /// <summary>
        /// Called when the loading state has changed. This callback will be executed twice
        /// once when loading is initiated either programmatically or by user action,
        /// and once when loading is terminated due to completion, cancellation of failure.
        /// This method will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="loadingStateChangedArgs">args</param>
        void OnLoadingStateChange(IWebBrowser chromiumWebBrowser, LoadingStateChangedEventArgs loadingStateChangedArgs);

        /// <summary>
        /// Called when the browser begins loading a frame.
        /// The <see cref="FrameLoadEndEventArgs.Frame"/> value will never be empty
        /// Check the <see cref="IFrame.IsMain"/> method to see if this frame is the main frame.
        /// Multiple frames may be loading at the same time. Sub-frames may start or continue loading after the main frame load has ended.
        /// This method may not be called for a particular frame if the load request for that frame fails.
        /// For notification of overall browser load status use <see cref="OnLoadingStateChange"/> instead. 
        /// This method will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="frameLoadStartArgs">args</param>
        /// <remarks>Whilst thist may seem like a logical place to execute js, it's called before the DOM has been loaded, implement
        /// <see cref="IRenderProcessMessageHandler.OnContextCreated"/> as it's called when the underlying V8Context is created
        /// (Only called for the main frame at this stage)</remarks>
        void OnFrameLoadStart(IWebBrowser chromiumWebBrowser, FrameLoadStartEventArgs frameLoadStartArgs);

        /// <summary>
        /// Called when the browser is done loading a frame.
        /// The <see cref="FrameLoadEndEventArgs.Frame"/> value will never be empty
        /// Check the <see cref="IFrame.IsMain"/> method to see if this frame is the main frame.
        /// Multiple frames may be loading at the same time. Sub-frames may start or continue loading after the main frame load has ended.
        /// This method will always be called for all frames irrespective of whether the request completes successfully. 
        /// This method will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="frameLoadEndArgs">args</param>
        void OnFrameLoadEnd(IWebBrowser chromiumWebBrowser, FrameLoadEndEventArgs frameLoadEndArgs);

        /// <summary>
        /// Called when the resource load for a navigation fails or is canceled.
        /// <see cref="LoadErrorEventArgs.ErrorCode"/> is the error code number, <see cref="LoadErrorEventArgs.ErrorText"/> is the error text and
        /// <see cref="LoadErrorEventArgs.FailedUrl"/> is the URL that failed to load. See net\base\net_error_list.h
        /// for complete descriptions of the error codes.
        /// This method will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="loadErrorArgs">args</param>
        void OnLoadError(IWebBrowser chromiumWebBrowser, LoadErrorEventArgs loadErrorArgs);
    }
}
