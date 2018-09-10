// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Javascript exception
    /// </summary>
    public class JavascriptException
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Stack trace in javascript frames
        /// </summary>
        public JavascriptStackFrame[] StackTrace { get; set; }
    }
}
