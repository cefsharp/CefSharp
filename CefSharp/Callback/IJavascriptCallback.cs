// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Javascript callback interface
    /// </summary>
    public interface IJavascriptCallback : IDisposable
    {
        /// <summary>
        /// Callback Id
        /// </summary>
        Int64 Id { get; }

        /// <summary>
        /// Execute the javascript callback
        /// </summary>
        /// <param name="parms">param array of objects</param>
        /// <returns>JavascriptResponse</returns>
        Task<JavascriptResponse> ExecuteAsync(params object[] parms);

        /// <summary>
        /// Execute the javascript callback
        /// </summary>
        /// <param name="parms">param array of objects</param>
        /// <returns>JavascriptResponse</returns>
        Task<JavascriptResponse> ExecuteWithTimeoutAsync(TimeSpan? timeout, params object[] parms);

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
