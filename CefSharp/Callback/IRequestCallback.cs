// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IRequestCallback : IDisposable
    {
        /// <summary>
        /// Continue the url request. 
        /// </summary>
        /// <param name="allow">If is true the request will be continued, otherwise, the request will be canceled.</param>
        void Continue(bool allow);
        
        /// <summary>
        /// Cancel the url request.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
