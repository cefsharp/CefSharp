// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        /// Takes the epoch and creates a <see cref="DateTime"/>
        /// </summary>
        /// <param name="epoch">cef datetime represented as difference between 01/01/1970</param>
        /// <returns></returns>
        public static DateTime FromCefTime(double epoch)
        {
            if (epoch > 0)
            {
                return FirstOfTheFirstNineteenSeventy.AddSeconds(epoch).ToLocalTime();
            }
            return DateTime.MinValue;
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
