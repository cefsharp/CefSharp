// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Generic callback interface used for asynchronous continuation.
    /// </summary>
    /// <remarks>
    /// All callback objects self-dispose on <see cref="Continue"/> or <see cref="Cancel"/>.
    /// You should not dispose of these objects in your own code because they
    /// manage native CEF resources and may need to be disposed on specific threads.
    /// </remarks>
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
