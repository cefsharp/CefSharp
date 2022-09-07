// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Mapping to/from CefBaseTime
    /// </summary>
    public static class CefTimeUtils
    {
        /// <summary>
        /// Converts from CefBaseTime to DateTime?
        /// </summary>
        /// <param name="val">
        /// Represents a wall clock time in UTC. Values are not guaranteed to be monotonically
        /// non-decreasing and are subject to large amounts of skew. Time is stored internally
        /// as microseconds since the Windows epoch (1601).
        /// </param>
        /// <returns>if <paramref name="val"/> is 0 then returns null otherwise returns a <see cref="DateTime"/> of <see cref="DateTimeKind.Local"/></returns>
        public static DateTime? FromBaseTimeToNullableDateTime(long val)
        {
            if(val == 0)
            {
                return null;
            }

            return DateTime.FromFileTime(val * 10);
        }

        /// <summary>
        /// Converts from CefBaseTime to DateTime
        /// </summary>
        /// <param name="val">
        /// Represents a wall clock time in UTC. Values are not guaranteed to be monotonically
        /// non-decreasing and are subject to large amounts of skew. Time is stored internally
        /// as microseconds since the Windows epoch (1601).
        /// </param>
        /// <returns>returns a <see cref="DateTime"/> of <see cref="DateTimeKind.Local"/></returns>
        public static DateTime FromBaseTimeToDateTime(long val)
        {
            return DateTime.FromFileTime(val * 10);
        }

        /// <summary>
        /// Converts from DateTime to CefBaseTime
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>
        /// Represents a wall clock time in UTC. Time as microseconds since the Windows epoch (1601).
        /// </returns>
        public static long FromDateTimeToBaseTime(DateTime dateTime)
        {
            // Same as calling ToFileTime, this to me is a little
            // more self descriptive of what's going on.
            return dateTime.ToUniversalTime().ToFileTimeUtc() / 10;
        }
    }
}
