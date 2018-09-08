// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public static class CefTimeUtils
    {
        public static DateTime ConvertCefTimeToDateTime(double epoch)
        {
            if (epoch == 0)
            {
                return DateTime.MinValue;
            }
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
        }
    }
}
