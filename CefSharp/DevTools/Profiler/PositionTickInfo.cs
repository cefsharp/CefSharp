// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Specifies a number of samples attributed to a certain source position.
    /// </summary>
    public class PositionTickInfo
    {
        /// <summary>
        /// Source line number (1-based).
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Number of samples attributed to the source line.
        /// </summary>
        public int Ticks
        {
            get;
            set;
        }
    }
}