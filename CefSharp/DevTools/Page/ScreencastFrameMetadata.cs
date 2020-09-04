// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Screencast frame metadata.
    /// </summary>
    public class ScreencastFrameMetadata
    {
        /// <summary>
        /// Top offset in DIP.
        /// </summary>
        public long OffsetTop
        {
            get;
            set;
        }

        /// <summary>
        /// Page scale factor.
        /// </summary>
        public long PageScaleFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Device screen width in DIP.
        /// </summary>
        public long DeviceWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Device screen height in DIP.
        /// </summary>
        public long DeviceHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Position of horizontal scroll in CSS pixels.
        /// </summary>
        public long ScrollOffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Position of vertical scroll in CSS pixels.
        /// </summary>
        public long ScrollOffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Frame swap timestamp.
        /// </summary>
        public long? Timestamp
        {
            get;
            set;
        }
    }
}