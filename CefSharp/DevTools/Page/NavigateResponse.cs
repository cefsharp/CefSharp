// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// NavigateResponse
    /// </summary>
    public class NavigateResponse
    {
        /// <summary>
        /// Frame id that has navigated (or failed to navigate)
        /// </summary>
        public string frameId
        {
            get;
            set;
        }

        /// <summary>
        /// Loader identifier.
        /// </summary>
        public string loaderId
        {
            get;
            set;
        }

        /// <summary>
        /// User friendly error message, present if and only if navigation has failed.
        /// </summary>
        public string errorText
        {
            get;
            set;
        }
    }
}