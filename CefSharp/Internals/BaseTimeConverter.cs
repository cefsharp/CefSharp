// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <inheritdoc/>
    public sealed class BaseTimeConverter : IBaseTimeConverter
    {
        /// <inheritdoc/>
        DateTime IBaseTimeConverter.FromBaseTimeToDateTime(long val)
        {
            const long MaxFileTime = 2650467743999999999;

            var fileTime = val * 10;

            if (fileTime > MaxFileTime)
            {
                return DateTime.MaxValue;
            }

            return DateTime.FromFileTime(fileTime);
        }

        /// <inheritdoc/>
        long IBaseTimeConverter.FromDateTimeToBaseTime(DateTime dateTime)
        {
            // Same as calling ToFileTime, this to me is a little
            // more self descriptive of what's going on.
            return dateTime.ToUniversalTime().ToFileTimeUtc() / 10;
        }
    }
}
