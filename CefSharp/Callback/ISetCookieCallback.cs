// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Interface to implement to be notified of asynchronous completion via ICookieManager.SetCookie().
    /// It will be executed asnychronously on the CEF IO thread after the cookie has been set
    /// </summary>
    public interface ISetCookieCallback : IDisposable
    {
        /// <summary>
        /// Method that will be called upon completion. 
        /// </summary>
        /// <param name="success">success will be true if the cookie was set successfully.</param>
        void OnComplete(bool success);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

