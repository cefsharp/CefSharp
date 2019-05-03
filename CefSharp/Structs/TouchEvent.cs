// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;

namespace CefSharp.Structs
{
    /// <summary>
    /// Touch Event
    /// </summary>
    public struct TouchEvent
    {
        /// <summary>
        /// Id of a touch point. Must be unique per touch, can be any number except -1.
        /// Note that a maximum of 16 concurrent touches will be tracked; touches
        /// beyond that will be ignored.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// X coordinate relative to the left side of the view.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y coordinate relative to the top side of the view.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// X radius in pixels. Set to 0 if not applicable.
        /// </summary>
        public float RadiusX { get; set; }

        /// <summary>
        /// Y radius in pixels. Set to 0 if not applicable.
        /// </summary>
        public float RadiusY { get; set; }

        /// <summary>
        /// Rotation angle in radians. Set to 0 if not applicable.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// The device type that caused the event.
        /// </summary>
        public PointerType PointerType { get; set; }

        /// <summary>
        /// The normalized pressure of the pointer input in the range of [0,1].
        /// Set to 0 if not applicable.
        /// </summary>
        public float Pressure { get; set; }

        /// <summary>
        /// The state of the touch point. Touches begin with one <see cref="TouchEventType.Pressed"/> event
        /// followed by zero or more <see cref="TouchEventType.Moved"/> events and finally one
        /// <see cref="TouchEventType.Released"/> or <see cref="TouchEventType.Cancelled"/> event.
        /// Events not respecting this order will be ignored.
        /// </summary>
        public TouchEventType Type { get; set; }

        /// <summary>
        ///  Bit flags describing any pressed modifier keys.
        /// </summary>
        public CefEventFlags Modifiers { get; set; }
    }
}
