// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Represents the state of a setting.
    /// </summary>
    public enum CefState
    {
        /// <summary>
        /// Use the default state for the setting.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Enable or allow the setting.
        /// </summary>
        Enabled,
        /// <summary>
        /// Disable or disallow the setting.
        /// </summary>
        Disabled,
    }
}
