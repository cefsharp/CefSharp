// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Represents an IME composition underline.
    /// </summary>
    public struct CompositionUnderline
    {
        /// <summary>
        /// Underline character range.
        /// </summary>
        public Range Range { get; private set; }

        /// <summary>
        /// Text color. 32-bit ARGB color value, not premultiplied. The color components are always
        /// in a known order. Equivalent to the SkColor type.
        /// </summary>
        public uint Color { get; private set; }
        /// <summary>
        /// Background color. 32-bit ARGB color value, not premultiplied. The color components are always
        /// in a known order. Equivalent to the SkColor type.
        /// </summary>
        public uint BackgroundColor { get; private set; }

        /// <summary>
        /// true for thickunderline
        /// </summary>
        public bool Thick { get; private set; }

        public CompositionUnderline(Range range, uint color, uint backGroundColor, bool thick)
            : this()
        {
            Range = range;
            Color = color;
            BackgroundColor = backGroundColor;
            Thick = thick;
        }                
    }
}

