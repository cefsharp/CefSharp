// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Visual viewport position, dimensions, and scale.
    /// </summary>
    public class VisualViewport
    {
        /// <summary>
        /// Horizontal offset relative to the layout viewport (CSS pixels).
        /// </summary>
        public long OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the layout viewport (CSS pixels).
        /// </summary>
        public long OffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal offset relative to the document (CSS pixels).
        /// </summary>
        public long PageX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the document (CSS pixels).
        /// </summary>
        public long PageY
        {
            get;
            set;
        }

        /// <summary>
        /// Width (CSS pixels), excludes scrollbar if present.
        /// </summary>
        public long ClientWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Height (CSS pixels), excludes scrollbar if present.
        /// </summary>
        public long ClientHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Scale relative to the ideal viewport (size at width=device-width).
        /// </summary>
        public long Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Page zoom factor (CSS to device independent pixels ratio).
        /// </summary>
        public long Zoom
        {
            get;
            set;
        }
    }
}