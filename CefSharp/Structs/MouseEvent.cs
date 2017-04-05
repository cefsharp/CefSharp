// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Struct representing a mouse event.
    /// </summary>
    public struct MouseEvent
    {
        /// <summary>
        /// x coordinate - relative to upper-left corner of view
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// y coordinate - relative to upper-left corner of view
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Bit flags describing any pressed modifier keys.
        /// </summary>
        public CefEventFlags Modifiers { get; private set; }

        public MouseEvent(int x, int y, CefEventFlags modifiers) : this()
        {
            X = x;
            Y = y;
            Modifiers = modifiers;
        }
    }
}
