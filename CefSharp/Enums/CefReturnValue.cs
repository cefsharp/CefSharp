// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Return value types.
    /// </summary>
    public enum CefReturnValue
    {
        /// <summary>
        /// Cancel immediately.
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// Continue immediately.
        /// </summary>
        Continue,

        /// <summary>
        /// Continue asynchronously (usually via a callback).
        /// </summary>
        ContinueAsync
    }
}