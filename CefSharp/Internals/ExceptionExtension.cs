// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CefSharp.Internals
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// The default log count for inner exceptions.
        /// </summary>
        public const int InnerExceptionLogCount = 5;

        /// <summary>
        /// Creates a detailed expection string from a provided exception.
        /// </summary>
        /// <param name="ex">The exception which will be used as base for the message</param>
        /// <param name="limit">The optional limit for logging recrusive inner exceptions.
        /// Default value is: INNER_EXCEPTION_LOG_COUNT</param>
        [DebuggerStepThrough]
        public static String ToDetailedString(this Exception ex, int limit = InnerExceptionLogCount)
        {
            var innerException = ex.InnerException;
            if (innerException != null && limit > 0)
            {
                limit = limit - 1;
                return ex.Message + ":\n" + ex.StackTrace + "\ncaused by:\n" + innerException.ToDetailedString(limit);
            }
            return ex.Message + ":\n" + ex.StackTrace;
        }
    }
}
