// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
namespace CefSharp.Internals
{
    /// <summary>
    /// Mapping to/from CefTime
    /// </summary>
    public static class DateTimeUtils
    {
        private static DateTime FirstOfTheFirstNineteenSeventy = new DateTime(1970, 1, 1, 0, 0, 0);

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
