// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Generic callback interface used for asynchronous completion. 
    /// </summary>
    public interface ICompletionCallback : IDisposable
    {
        /// <summary>
        /// Method that will be called once the task is complete. 
        /// </summary>
        void OnComplete();

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
