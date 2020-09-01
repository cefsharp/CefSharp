// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Box model.
    /// </summary>
    public class BoxModel
    {
        /// <summary>
        /// Content box
        /// </summary>
        public long Content
        {
            get;
            set;
        }

        /// <summary>
        /// Padding box
        /// </summary>
        public long Padding
        {
            get;
            set;
        }

        /// <summary>
        /// Border box
        /// </summary>
        public long Border
        {
            get;
            set;
        }

        /// <summary>
        /// Margin box
        /// </summary>
        public long Margin
        {
            get;
            set;
        }

        /// <summary>
        /// Node width
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Node height
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Shape outside coordinates
        /// </summary>
        public ShapeOutsideInfo ShapeOutside
        {
            get;
            set;
        }
    }
}