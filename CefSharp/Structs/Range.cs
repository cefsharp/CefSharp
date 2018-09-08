// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    /// <summary>
    /// Represents a range
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// From
        /// </summary>
        public int From { get; private set; }

        /// <summary>
        /// To
        /// </summary>
        public int To { get; private set; }

        /// <summary>
        /// Range
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        public Range(int from, int to)
            : this()
        {
            From = from;
            To = to;
        }
    }
}
