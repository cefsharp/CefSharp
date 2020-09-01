// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Viewport for capturing screenshot.
    /// </summary>
    public class Viewport
    {
        /// <summary>
        /// X offset in device independent pixels (dip).
        /// </summary>
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Y offset in device independent pixels (dip).
        /// </summary>
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle width in device independent pixels (dip).
        /// </summary>
        public long Width
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle height in device independent pixels (dip).
        /// </summary>
        public long Height
        {
            get;
            set;
        }

        /// <summary>
        /// Page scale factor.
        /// </summary>
        public long Scale
        {
            get;
            set;
        }
    }
}