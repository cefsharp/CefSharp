// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used to asynchronously cancel a download.
    /// </summary>
    public interface IDownloadItemCallback : IDisposable
    {
        /// <summary>
        /// Call to cancel the download.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Call to pause the download.
        /// </summary>
        void Pause();

        /// <summary>
        /// Call to resume the download.
        /// </summary>
        void Resume();

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
