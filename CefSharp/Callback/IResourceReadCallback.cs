// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Callback
{
    /// <summary>
    /// Callback for asynchronous continuation of <see cref="IResourceHandler.Read"/>.
    /// </summary>
    public interface IResourceReadCallback : IDisposable
    {
        /// <summary>
        /// Callback for asynchronous continuation of <see cref="IResourceHandler.Read"/>. If bytesRead == 0
        /// the response will be considered complete. 
        /// </summary>
        /// <param name="bytesRead">
        /// If bytesRead == 0 the response will be considered complete.
        /// If bytesRead &gt; 0 then <see cref="IResourceHandler.Read"/> will be called again until the request is complete (based on either the
        /// result or the expected content length). If bytesRead &lt; 0 then the
        /// request will fail and the bytesRead value will be treated as the error
        /// code.
        /// </param>
        void Continue(int bytesRead);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

