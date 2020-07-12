// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Structs
{
    /// <summary>
    /// Struct representing cursor information.
    /// </summary>
    public struct CursorInfo
    {
        /// <summary>
        /// Cursor buffer
        /// </summary>
        public IntPtr Buffer { get; set; }
        /// <summary>
        /// Hotspot
        /// </summary>
        public Point Hotspot { get; private set; }
        /// <summary>
        /// Image scale factor
        /// </summary>
        public float ImageScaleFactor { get; private set; }
        /// <summary>
        /// Size
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// CursorInfo
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="hotspot">hotspot</param>
        /// <param name="imageScaleFactor">image scale factor</param>
        /// <param name="size">size</param>
        public CursorInfo(IntPtr buffer, Point hotspot, float imageScaleFactor, Size size)
        {
            Buffer = buffer;
            Hotspot = hotspot;
            ImageScaleFactor = imageScaleFactor;
            Size = size;
        }
    }
}
