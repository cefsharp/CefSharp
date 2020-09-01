// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// Screen orientation.
    /// </summary>
    public class ScreenOrientation
    {
        /// <summary>
        /// Orientation type.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Orientation angle.
        /// </summary>
        public int Angle
        {
            get;
            set;
        }
    }
}