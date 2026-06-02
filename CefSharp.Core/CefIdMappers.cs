// Copyright © 2026 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Provides version-safe mapping of Chromium string identifiers to their version-specific numeric ID values.
    /// </summary>
    public static class CefIdMappers
    {
        /// <summary>
        /// Returns the numeric ID value for an IDC <paramref name="name"/> from cef_command_ids.h or -1
        /// if <paramref name="name"/> is unrecognized by the current CEF/Chromium build.
        /// </summary>
        /// <remarks>
        /// This function provides version-safe mapping of command IDC names to version-specific
        /// numeric ID values. Numeric ID values are likely to change across
        /// CEF/Chromium versions but names generally remain the same.
        /// </remarks>
        /// <param name="name">String identifier of the Chromium command.</param>
        /// <returns>version-specific numeric ID value for the command if recognized; otherwise, -1.</returns>
        public static int CefIdForCommandIdName(string name)
        {
            return CefSharp.Core.CefIdMappers.CefIdForCommandIdName(name);
        }
    }
}
