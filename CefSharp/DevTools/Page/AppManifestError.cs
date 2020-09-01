// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Error while paring app manifest.
    /// </summary>
    public class AppManifestError
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// If criticial, this is a non-recoverable parse error.
        /// </summary>
        public int Critical
        {
            get;
            set;
        }

        /// <summary>
        /// Error line.
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Error column.
        /// </summary>
        public int Column
        {
            get;
            set;
        }
    }
}