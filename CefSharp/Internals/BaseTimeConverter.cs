// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <inheritdoc/>
    public sealed class BaseTimeConverter : IBaseTimeConverter
    {
        private static DateTime UtcWindowsEpoch = DateTime.SpecifyKind(DateTime.Parse("1601-01-01"), DateTimeKind.Utc);

        /// <inheritdoc/>
        DateTime IBaseTimeConverter.FromBaseTimeToDateTime(long val)
        {
            //DateTime.MaxTicks - DateTime.FileTimeOffset
            const long MaxFileTime = 2650467743999999999;
            //DateTime.FileTimeOffset
            const long FileTimeOffset = 504911232000000000;

            var fileTime = val * 10;

            if (fileTime > MaxFileTime)
            {
                return DateTime.MaxValue;
            }

            if (fileTime < 0)
            {
                var universalTicks = fileTime + FileTimeOffset;

                if(universalTicks <= 0)
                {
                    return DateTime.MinValue.ToLocalTime();
                }

                return new DateTime(universalTicks, DateTimeKind.Utc).ToLocalTime();
            }

            return DateTime.FromFileTime(fileTime);
        }

        /// <inheritdoc/>
        long IBaseTimeConverter.FromDateTimeToBaseTime(DateTime dateTime)
        {
            //DateTime.FileTimeOffset
            const long FileTimeOffset = 504911232000000000;

            var utcDateTime = dateTime.ToUniversalTime();

            // FileTime doesn't support less than Epoch
            if(utcDateTime < UtcWindowsEpoch)
            {
                var ticks = utcDateTime.Ticks - FileTimeOffset;

                return ticks / 10;
            }

            // Same as calling ToFileTime, this to me is a little
            // more self descriptive of what's going on.
            return utcDateTime.ToFileTimeUtc() / 10;
        }
    }
}
