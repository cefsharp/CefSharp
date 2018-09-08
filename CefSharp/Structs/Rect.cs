// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    /// <summary>
    /// Represents a rectangle
    /// </summary>
    public struct Rect
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Rect
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public Rect(int x, int y, int width, int height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
