// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to geolocation permission requests.
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public interface IGeolocationHandler
    {
        /// <summary>
        /// Called when a page requests permission to access geolocation information.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="requestingUrl">the URL requesting permission</param>
        /// <param name="requestId">the unique ID for the permission request</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of geolocation permission requests.</param>
        /// <returns>Return true and call IGeolocationCallback.Continue() either in this method or at a later time to continue or cancel the request. Return false to cancel the request immediately.</returns>
        bool OnRequestGeolocationPermission(IWebBrowser browserControl, IBrowser browser, string requestingUrl, int requestId, IGeolocationCallback callback);

        /// <summary>
        /// Called when a geolocation access request is canceled.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="requestId">the unique ID for the permission request, as seen in <see cref="OnRequestGeolocationPermission"/></param>
        void OnCancelGeolocationPermission(IWebBrowser browserControl, IBrowser browser, int requestId);
    }
}
