// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// CSS Shape Outside details.
    /// </summary>
    public class ShapeOutsideInfo
    {
        /// <summary>
        /// Shape bounds
        /// </summary>
        public long Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Shape coordinate details
        /// </summary>
        public object Shape
        {
            get;
            set;
        }

        /// <summary>
        /// Margin shape bounds
        /// </summary>
        public object MarginShape
        {
            get;
            set;
        }
    }
}