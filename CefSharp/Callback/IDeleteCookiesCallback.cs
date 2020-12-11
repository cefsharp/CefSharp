// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Interface to implement to be notified of asynchronous completion via ICookieManager.DeleteCookies().
    /// It will be executed asynchronously on the CEF IO thread after the cookie has been deleted
    /// </summary>
    public interface IDeleteCookiesCallback : IDisposable
    {
        /// <summary>
        /// Method that will be called upon completion. 
        /// </summary>
        /// <param name="numDeleted">will be the number of cookies that were deleted or -1 if unknown.</param>
        void OnComplete(int numDeleted);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}

