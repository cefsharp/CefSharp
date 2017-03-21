// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
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

        public Range(int from, int to)
            : this()
        {
            From = from;
            To = to;
        }
    }
}
