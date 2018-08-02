// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    public struct ScreenInfo
    {
        public float ScaleFactor { get; private set; }

        public ScreenInfo(float scaleFactor) : this()
        {
            ScaleFactor = scaleFactor;
        }

        /// <summary>
        /// Height of the monitor nearest to the window hosting the browser
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width of the monitor nearest to the window hosting the browser
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Available Left location  of the monitor nearest to the window hosting the browser.  iF toolbar is on the left - this would be a non-zero value
        /// </summary>
        public int AvailableLeft { get; set; }

        /// <summary>
        /// Available Top location  of the monitor nearest to the window hosting the browser.  iF toolbar is on top - this would be a non-zero value
        /// </summary>
        public int AvailableTop { get; set; }

        /// <summary>
        /// Available Wdith of the monitor nearest to the window hosting the browser.  iF toolbar is on the left or on the right - this would be different from the <see cref="Width">Width</see> property
        /// </summary>
        public int AvailableWidth { get; set; }

        /// <summary>
        /// Available Heiht of the monitor nearest to the window hosting the browser.  iF toolbar is on top or on the bottom - this would be different from the <see cref="Height">Height</see> property
        /// </summary>
        public int AvailableHeight { get; set; }
    }
}
