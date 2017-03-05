// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of geolocation permission requests.
    /// </summary>
    public interface IGeolocationCallback : IDisposable
    {
        /// <summary>
        /// Call to allow or deny geolocation access.
        /// </summary>
        /// <param name="allow">true to allow</param>
        void Continue(bool allow);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
