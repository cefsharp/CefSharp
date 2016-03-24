// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class representing a mouse event.
    /// </summary>
    public class MouseEvent
    {
        /// <summary>
        /// X coordinate relative to the left side of the view.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate relative to the top side of the view.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Bit flags describing any pressed modifier keys.
        /// </summary>
        public CefEventFlags Modifiers { get; set; }
    }
}
