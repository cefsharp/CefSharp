// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    public interface IJavascriptCallback : IDisposable
    {
        Task<JavascriptResponse> ExecuteAsync(params object[] parms);

        /// <summary>
        /// Check to see if the underlying resource are still available to execute the callback
        /// </summary>
        bool CanExecute { get; }

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
