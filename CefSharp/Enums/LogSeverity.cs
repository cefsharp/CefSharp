// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public enum LogSeverity
    {
        /// <summary>
        /// Default logging (currently Info logging)
        /// </summary>
        Default = 0,

        /// <summary>
        /// Verbose logging.
        /// </summary>
        Verbose,

        /// <summary>
        /// Info logging
        /// </summary>
        Info,

        /// <summary>
        /// Warning logging
        /// </summary>
        Warning,

        /// <summary>
        /// Error logging
        /// </summary>
        Error,

        /// <summary>
        /// Fatal logging.
        /// </summary>
        Fatal,

        /// <summary>
        /// Disable logging to file for all messages, and to stderr for messages with severity less than FATAL.
        /// </summary>
        Disable = 99
    };
}
