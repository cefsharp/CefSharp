// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    public struct ResolveCallbackResult
    {
        public CefErrorCode Result { get; private set; }
        public IList<string> ResolvedIpAddresses { get; private set; }

        public ResolveCallbackResult(CefErrorCode result, IList<string> resolvedIpAddresses) : this()
        {
            Result = result;
            ResolvedIpAddresses = resolvedIpAddresses;
        }
    }
}
