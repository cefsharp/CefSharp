// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to filter cookies that may be sent or received from
    /// resource requests. The methods of this class will be called on the CEF IO thread
    /// unless otherwise indicated.
    /// </summary>
    public interface ICookieAccessFilter
    {
        /// <summary>
        /// Called on the CEF IO thread before a resource request is sent.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="cookie">the cookie object</param>
        /// <returns>Return true if the specified cookie can be sent with the request or false otherwise.</returns>
        bool CanSendCookie(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, Cookie cookie);

        /// <summary>
        /// Called on the CEF IO thread after a resource response is received.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <param name="cookie">the cookie object</param>
        /// <returns>Return true if the specified cookie returned with the response can be saved or false otherwise.</returns>
        bool CanSaveCookie(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, Cookie cookie);
    }
}
