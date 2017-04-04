// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Representing a draggable region.
    /// </summary>
    public struct DraggableRegion
    {
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Is this region draggable
        /// </summary>
        public bool Draggable { get; private set; }

        /// <summary>
        /// Creates a new DraggableRegion
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="draggable">is draggable?</param>
        public DraggableRegion(int width, int height, int x, int y, bool draggable) : this()
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
            Draggable = draggable;
        }
    }
}
