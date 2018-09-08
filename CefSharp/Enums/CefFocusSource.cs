// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Focus Source
    /// </summary>
    public enum CefFocusSource
    {
        /// <summary>
        /// The source is explicit navigation via the API (LoadURL(), etc).
        /// </summary>
        FocusSourceNavigation = 0,
        /// <summary>
        /// The source is a system-generated focus event.
        /// </summary>
        FocusSourceSystem
    }
}
