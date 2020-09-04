// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetNavigationHistoryResponse
    /// </summary>
    public class GetNavigationHistoryResponse
    {
        /// <summary>
        /// Index of the current navigation history entry.
        /// </summary>
        public int currentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Array of navigation history entries.
        /// </summary>
        public System.Collections.Generic.IList<NavigationEntry> entries
        {
            get;
            set;
        }
    }
}