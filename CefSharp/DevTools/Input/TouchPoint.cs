// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    /// <summary>
    /// TouchPoint
    /// </summary>
    public class TouchPoint
    {
        /// <summary>
        /// X coordinate of the event relative to the main frame's viewport in CSS pixels.
        /// </summary>
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Y coordinate of the event relative to the main frame's viewport in CSS pixels. 0 refers to
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// X radius of the touch area (default: 1.0).
        /// </summary>
        public long? RadiusX
        {
            get;
            set;
        }

        /// <summary>
        /// Y radius of the touch area (default: 1.0).
        /// </summary>
        public long? RadiusY
        {
            get;
            set;
        }

        /// <summary>
        /// Rotation angle (default: 0.0).
        /// </summary>
        public long? RotationAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Force (default: 1.0).
        /// </summary>
        public long? Force
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier used to track touch sources between events, must be unique within an event.
        /// </summary>
        public long? Id
        {
            get;
            set;
        }
    }
}