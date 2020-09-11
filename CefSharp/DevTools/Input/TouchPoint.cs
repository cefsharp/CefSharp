// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    /// <summary>
    /// TouchPoint
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class TouchPoint : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// X coordinate of the event relative to the main frame's viewport in CSS pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("x"), IsRequired = (true))]
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Y coordinate of the event relative to the main frame's viewport in CSS pixels. 0 refers to
        /// the top of the viewport and Y increases as it proceeds towards the bottom of the viewport.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("y"), IsRequired = (true))]
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// X radius of the touch area (default: 1.0).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("radiusX"), IsRequired = (false))]
        public long? RadiusX
        {
            get;
            set;
        }

        /// <summary>
        /// Y radius of the touch area (default: 1.0).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("radiusY"), IsRequired = (false))]
        public long? RadiusY
        {
            get;
            set;
        }

        /// <summary>
        /// Rotation angle (default: 0.0).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rotationAngle"), IsRequired = (false))]
        public long? RotationAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Force (default: 1.0).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("force"), IsRequired = (false))]
        public long? Force
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier used to track touch sources between events, must be unique within an event.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (false))]
        public long? Id
        {
            get;
            set;
        }
    }
}