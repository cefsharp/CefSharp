// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Layout viewport position and dimensions.
    /// </summary>
    public class LayoutViewport
    {
        /// <summary>
        /// Horizontal offset relative to the document (CSS pixels).
        /// </summary>
        public int PageX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the document (CSS pixels).
        /// </summary>
        public int PageY
        {
            get;
            set;
        }

        /// <summary>
        /// Width (CSS pixels), excludes scrollbar if present.
        /// </summary>
        public int ClientWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Height (CSS pixels), excludes scrollbar if present.
        /// </summary>
        public int ClientHeight
        {
            get;
            set;
        }
    }
}