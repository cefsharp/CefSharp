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
        Unknown = 0,
        /// <summary>
        /// An enum constant representing the current tab option.
        /// </summary>
        CurrentTab = 1,
        /// <summary>
        /// Indicates that only one tab with the url should exist in the same window
        /// </summary>
        SingletonTab = 2,
        /// <summary>
        /// An enum constant representing the new foreground tab option.
        /// </summary>
        NewForegroundTab = 3,
        /// <summary>
        /// An enum constant representing the new background tab option.
        /// </summary>
        NewBackgroundTab = 4,
        /// <summary>
        /// An enum constant representing the new popup option.
        /// </summary>
        NewPopup = 5,
        /// <summary>
        /// An enum constant representing the new window option.
        /// </summary>
        NewWindow = 6,
        /// <summary>
        /// An enum constant representing the save to disk option.
        /// </summary>
        SaveToDisk = 7,
        /// <summary>
        /// An enum constant representing the off the record option.
        /// </summary>
        OffTheRecord = 8,
        /// <summary>
        /// An enum constant representing the ignore action option.
        /// </summary>
        IgnoreAction = 9,

        /// <summary>
		/// Activates an existing tab containing the url, rather than navigating.
		/// This is similar to SINGLETON_TAB, but searches across all windows from
		/// the current profile and anonymity (instead of just the current one);
		/// closes the current tab on switching if the current tab was the NTP with
		/// no session history; and behaves like CURRENT_TAB instead of
		/// NEW_FOREGROUND_TAB when no existing tab is found.
		/// </summary>
		SwitchToTab = 10,

        /// <summary>
        /// Creates a new document picture-in-picture window showing a child WebView.
        /// </summary>
        NewPictureInPicture = 11,
    }
}
