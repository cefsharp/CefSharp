// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Convert to/from <see cref="DateTime"/> and CefBaseTime
    /// </summary>
    public interface IBaseTimeConverter
    {
        /// <summary>
        /// Converts from CefBaseTime to DateTime
        /// </summary>
        /// <param name="val">
        /// Represents a wall clock time in UTC. Values are not guaranteed to be monotonically
        /// non-decreasing and are subject to large amounts of skew. Time is stored internally
        /// as microseconds since the Windows epoch (1601).
        /// </param>
        /// <returns>returns a <see cref="DateTime"/></returns>
        DateTime FromBaseTimeToDateTime(long val);

        /// <summary>
        /// Converts from DateTime to CefBaseTime
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>
        /// Represents a wall clock time in UTC. Time as microseconds since the Windows epoch (1601).
        /// </returns>
        long FromDateTimeToBaseTime(DateTime dateTime);
    }
}
