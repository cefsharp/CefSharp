﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to geolocation permission requests.
    /// The methods of this class will be called on the browser process UI thread. 
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
        /// <returns>true to allow the request and false to deny</returns>
        bool OnRequestGeolocationPermission(IWebBrowser browserControl, IBrowser browser, string requestingUrl, int requestId, IGeolocationCallback callback);


        /// <summary>
        /// Called when a geolocation access request is canceled.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="requestingUrl">the URL that originally requested permission</param>
        /// <param name="requestId">the unique ID for the permission request, as seen in <see cref="OnRequestGeolocationPermission"/></param>
        void OnCancelGeolocationPermission(IWebBrowser browserControl, IBrowser browser, string requestingUrl, int requestId);
    }
}
