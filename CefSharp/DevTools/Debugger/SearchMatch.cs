// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Search match for resource.
    /// </summary>
    public class SearchMatch
    {
        /// <summary>
        /// Line number in resource content.
        /// </summary>
        public long LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Line with match content.
        /// </summary>
        public string LineContent
        {
            get;
            set;
        }
    }
}