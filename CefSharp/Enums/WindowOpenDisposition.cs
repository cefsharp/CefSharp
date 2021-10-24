// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// The manner in which a link click should be opened.
    /// </summary>
    public enum WindowOpenDisposition
    {
        /// <summary>
        /// An enum constant representing the unknown option.
        /// </summary>
        Unknown,
        /// <summary>
        /// An enum constant representing the current tab option.
        /// </summary>
        CurrentTab,
        /// <summary>
        /// Indicates that only one tab with the url should exist in the same window
        /// </summary>
        SingletonTab,
        /// <summary>
        /// An enum constant representing the new foreground tab option.
        /// </summary>
        NewForegroundTab,
        /// <summary>
        /// An enum constant representing the new background tab option.
        /// </summary>
        NewBackgroundTab,
        /// <summary>
        /// An enum constant representing the new popup option.
        /// </summary>
        NewPopup,
        /// <summary>
        /// An enum constant representing the new window option.
        /// </summary>
        NewWindow,
        /// <summary>
        /// An enum constant representing the save to disk option.
        /// </summary>
        SaveToDisk,
        /// <summary>
        /// An enum constant representing the off the record option.
        /// </summary>
        OffTheRecord,
        /// <summary>
        /// An enum constant representing the ignore action option.
        /// </summary>
        IgnoreAction
    }
}
