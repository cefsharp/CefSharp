// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Generic callback interface used for asynchronous continuation.
    /// </summary>
    public interface ICallback : IDisposable
    {
        /// <summary>
        /// Continue processing.
        /// </summary>
        void Continue();

        /// <summary>
        /// Cancel processing.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
