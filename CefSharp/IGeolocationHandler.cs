// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IGeolocationHandler
    {
        /// <summary>
        /// Called when a page requests permission to access geolocation information.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="requestingUrl">the URL requesting permission</param>
        /// <param name="requestId">the unique ID for the permission request</param>
        /// <returns>true to allow the request and false to deny</returns>
        bool OnRequestGeolocationPermission(IWebBrowser browser, string requestingUrl, int requestId);

        /// <summary>
        /// Called when a geolocation access request is canceled.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="requestingUrl">the URL that originally requested permission</param>
        /// <param name="requestId">the unique ID for the permission request, as seen in <see cref="OnRequestGeolocationPermission"/></param>
        void OnCancelGeolocationPermission(IWebBrowser browser, string requestingUrl, int requestId);
    }
}
