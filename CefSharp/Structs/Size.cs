// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    /// <summary>
    /// Structure representing a size. 
    /// </summary>
    public struct Size
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
        /// Size
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public Size(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }
    }
}
