// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.InteropServices;
using CefSharp.Structs;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// </summary>
    /// <see cref="https://docs.microsoft.com/en-us/previous-versions/dd162897(v=vs.85)"/>
    /// <remarks>
    /// By convention, the right and bottom edges of the rectangle are normally considered exclusive. 
    /// In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the the rectangle. 
    /// For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including, 
    /// the right column and bottom row of pixels. This structure is identical to the RECTL structure.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectStruct
    {
        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom;

        public static implicit operator Rect(RectStruct rect)
        {
            return new Rect(0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }
    }
}
