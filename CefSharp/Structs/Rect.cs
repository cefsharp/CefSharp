// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;

namespace CefSharp.Structs
{
    /// <summary>
    /// Represents a rectangle
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}, Width = {Width}, Height = {Height}")]
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

        /// <summary>
        /// Returns a new Rect with Scaled values
        /// </summary>
        /// <param name="dpi">Dpi to scale by</param>
        /// <returns>New rect with scaled values</returns>
        public Rect ScaleByDpi(float dpi)
        {
            var x = (int)Math.Ceiling(X / dpi);
            var y = (int)Math.Ceiling(Y / dpi);
            var width = (int)Math.Ceiling(Width / dpi);
            var height = (int)Math.Ceiling(Height / dpi);

            return new Rect(x, y, width, height);
        }
    }
}
