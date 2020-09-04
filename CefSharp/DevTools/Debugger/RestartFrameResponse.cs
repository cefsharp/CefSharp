// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// RestartFrameResponse
    /// </summary>
    public class RestartFrameResponse
    {
        /// <summary>
        /// New stack trace.
        /// </summary>
        public System.Collections.Generic.IList<CallFrame> callFrames
        {
            get;
            set;
        }

        /// <summary>
        /// Async stack trace, if any.
        /// </summary>
        public Runtime.StackTrace asyncStackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Async stack trace, if any.
        /// </summary>
        public Runtime.StackTraceId asyncStackTraceId
        {
            get;
            set;
        }
    }
}