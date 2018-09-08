// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// IP Address solution callback result
    /// </summary>
    public struct ResolveCallbackResult
    {
        /// <summary>
        /// The result code - <see cref="CefErrorCode.None"/> on success
        /// </summary>
        public CefErrorCode Result { get; private set; }

        /// <summary>
        /// List of resolved IP addresses or empty if the resolution failed.
        /// </summary>
        public IList<string> ResolvedIpAddresses { get; private set; }

        /// <summary>
        /// ResolveCallbackResult
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="resolvedIpAddresses">list of ip addresses</param>
        public ResolveCallbackResult(CefErrorCode result, IList<string> resolvedIpAddresses) : this()
        {
            Result = result;
            ResolvedIpAddresses = resolvedIpAddresses;
        }
    }
}
