// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Permission request results.
    /// </summary>
    public enum PermissionRequestResult : uint
    {
        /// <summary>
        /// Accept the permission request as an explicit user action.
        /// </summary>
        Accept,

        /// <summary>
        /// Deny the permission request as an explicit user action.
        /// </summary>
        Deny,

        /// <summary>
        /// Dismiss the permission request as an explicit user action.
        /// </summary>
        Dismiss,

        /// <summary>
        /// Ignore the permission request. If the prompt remains unhandled (e.g.
        /// OnShowPermissionPrompt returns false and there is no default permissions
        /// UI) then any related promises may remain unresolved.
        /// </summary>
        Ignore,
    }
}
