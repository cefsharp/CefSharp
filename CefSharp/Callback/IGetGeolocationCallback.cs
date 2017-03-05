// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to receive geolocation updates. The methods of this
    /// class will be called on the CEF UI thread.
    /// </summary>
    public interface IGetGeolocationCallback : IDisposable
    {
        /// <summary>
        /// Called with the 'best available' location information or,
        /// if the location update failed, with error information.
        /// </summary>
        /// <param name="position">geo position</param>
        void OnLocationUpdate(Geoposition position);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}