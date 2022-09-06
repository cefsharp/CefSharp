// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Mapping to/from CefTime/CefBaseTime
    /// </summary>
    public static class CefTimeUtils
    {
        private static DateTime FirstOfTheFirstNineteenSeventy = new DateTime(1970, 1, 1, 0, 0, 0);

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

        public static DateTime ConvertCefTimeToDateTime(double epoch)
        {
            if (epoch == 0)
            {
                return DateTime.MinValue;
            }
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
        }

        /// <summary>
        /// Converts a cef
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        /// <param name="day">day</param>
        /// <param name="hour">hour</param>
        /// <param name="minute">minute</param>
        /// <param name="second">second</param>
        /// <param name="millisecond">millisecond</param>
        /// <returns>DateTime</returns>
        public static DateTime FromCefTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            try
            {
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Returns epoch (different from 01/01/1970)
        /// </summary>
        /// <param name="dateTime">datetime</param>
        /// <returns>epoch</returns>
        public static double ToCefTime(DateTime dateTime)
        {
            var timeSpan = dateTime - FirstOfTheFirstNineteenSeventy;

            return timeSpan.TotalSeconds;
        }
    }
}
