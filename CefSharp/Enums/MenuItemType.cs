// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Supported menu item types.
    /// </summary>
    public enum MenuItemType
    {
        /// <summary>
        /// An enum constant representing the none option.
        /// </summary>
        None = 0,
        /// <summary>
        /// An enum constant representing the command option.
        /// </summary>
        Command,
        /// <summary>
        /// An enum constant representing the check option.
        /// </summary>
        Check,
        /// <summary>
        /// An enum constant representing the radio option.
        /// </summary>
        Radio,
        /// <summary>
        /// An enum constant representing the separator option.
        /// </summary>
        Separator,
        /// <summary>
        /// An enum constant representing the sub menu option.
        /// </summary>
        SubMenu
    }
}
