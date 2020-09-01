// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a source range.
    /// </summary>
    public class CoverageRange
    {
        /// <summary>
        /// JavaScript script source offset for the range start.
        /// </summary>
        public int StartOffset
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script source offset for the range end.
        /// </summary>
        public int EndOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Collected execution count of the source range.
        /// </summary>
        public int Count
        {
            get;
            set;
        }
    }
}