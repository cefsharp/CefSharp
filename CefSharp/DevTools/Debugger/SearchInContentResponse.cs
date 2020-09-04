// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SearchInContentResponse
    /// </summary>
    public class SearchInContentResponse
    {
        /// <summary>
        /// List of search matches.
        /// </summary>
        public System.Collections.Generic.IList<SearchMatch> result
        {
            get;
            set;
        }
    }
}