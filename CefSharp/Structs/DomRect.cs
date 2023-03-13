// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    /// <summary>
    /// Dom Rect
    /// </summary>
    public struct DomRect
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X
        {
            get;
            private set;
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Rectangle width
        /// </summary>
        public double Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Rectangle height
        /// </summary>
        public double Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public DomRect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
