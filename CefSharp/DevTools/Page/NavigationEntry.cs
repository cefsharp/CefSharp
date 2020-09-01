// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Navigation history entry.
    /// </summary>
    public class NavigationEntry
    {
        /// <summary>
        /// Unique id of the navigation history entry.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the navigation history entry.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// URL that the user typed in the url bar.
        /// </summary>
        public string UserTypedURL
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the navigation history entry.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Transition type.
        /// </summary>
        public string TransitionType
        {
            get;
            set;
        }
    }
}