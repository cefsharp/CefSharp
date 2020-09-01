// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// DisplayFeature
    /// </summary>
    public class DisplayFeature
    {
        /// <summary>
        /// Orientation of a display feature in relation to screen
        /// </summary>
        public string Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// The offset from the screen origin in either the x (for vertical
        public int Offset
        {
            get;
            set;
        }

        /// <summary>
        /// A display feature may mask content such that it is not physically
        public int MaskLength
        {
            get;
            set;
        }
    }
}