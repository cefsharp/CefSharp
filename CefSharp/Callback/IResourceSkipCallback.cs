// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Callback
{
    /// <summary>
    /// Callback for asynchronous continuation of <see cref="IResourceHandler.Skip(long, long, IResourceSkipCallback)"/>.
    /// </summary>
    public interface IResourceSkipCallback : IDisposable
    {
        /// <summary>
        /// Callback for asynchronous continuation of Skip(). 
        /// </summary>
        /// <param name="bytesSkipped">If bytesSkipped &gt; 0 then either Skip() will be called
        /// again until the requested number of bytes have been skipped or the request will proceed.
        /// If bytesSkipped &lte; the request will fail with ERR_REQUEST_RANGE_NOT_SATISFIABLE.</param>
        void Continue(Int64 bytesSkipped);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

