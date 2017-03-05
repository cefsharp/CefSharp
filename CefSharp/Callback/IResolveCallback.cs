// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    public interface IResolveCallback
    {
        /// <summary>
        /// Called after the ResolveHost request has completed.
        /// </summary>
        /// <param name="result">The result code</param>
        /// <param name="resolvedIpAddresses">will be the list of resolved IP addresses or
        /// empty if the resolution failed.</param>
        void OnResolveCompleted(CefErrorCode result, IList<string> resolvedIpAddresses);
    }
}
