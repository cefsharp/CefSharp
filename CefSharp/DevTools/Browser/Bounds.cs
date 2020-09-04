// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Browser window bounds information
    /// </summary>
    public class Bounds
    {
        /// <summary>
        /// The offset from the left edge of the screen to the window in pixels.
        /// </summary>
        public int? Left
        {
            get;
            set;
        }

        /// <summary>
        /// The offset from the top edge of the screen to the window in pixels.
        /// </summary>
        public int? Top
        {
            get;
            set;
        }

        /// <summary>
        /// The window width in pixels.
        /// </summary>
        public int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// The window height in pixels.
        /// </summary>
        public int? Height
        {
            get;
            set;
        }

        /// <summary>
        /// The window state. Default to normal.
        /// </summary>
        public string WindowState
        {
            get;
            set;
        }
    }
}