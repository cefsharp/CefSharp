// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// Structure representing geoposition information. The properties of this
    /// structure correspond to those of the JavaScript Position object although
    /// their types may differ.
    /// </summary>
    public sealed class Geoposition
    {
        /// <summary>
        /// Latitude in decimal degrees north (WGS84 coordinate frame).
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude in decimal degrees west (WGS84 coordinate frame).
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Altitude in meters (above WGS84 datum).
        /// </summary>
        public double Altitude { get; set; }

        //Accuracy of horizontal position in meters.
        public double Accuracy { get; set; }

        /// <summary>
        /// Accuracy of altitude in meters.
        /// </summary>
        public double AltitudeAccuracy { get; set; }

        /// <summary>
        /// Heading in decimal degrees clockwise from true north.
        /// </summary>
        public double Heading { get; set; }

        /// <summary>
        /// Horizontal component of device velocity in meters per second.
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Time of position measurement in miliseconds since Epoch in UTC time. This
        /// is taken from the host computer's system clock.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Error code, see enum above.
        /// </summary>
        public CefGeoPositionErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Human-readable error message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
