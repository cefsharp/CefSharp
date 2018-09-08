// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Callback interface for <see cref="IRequestContext.ResolveHostAsync(Uri)"/>
    /// </summary>
    public interface IResolveCallback : IDisposable
    {
        /// <summary>
        /// Called after the ResolveHost request has completed.
        /// </summary>
        /// <param name="result">The result code</param>
        /// <param name="resolvedIpAddresses">will be the list of resolved IP addresses or
        /// empty if the resolution failed.</param>
        void OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
