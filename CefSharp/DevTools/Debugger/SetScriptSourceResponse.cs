// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetScriptSourceResponse
    /// </summary>
    public class SetScriptSourceResponse
    {
        /// <summary>
        /// New stack trace in case editing has happened while VM was stopped.
        /// </summary>
        public System.Collections.Generic.IList<CallFrame> callFrames
        {
            get;
            set;
        }

        /// <summary>
        /// Whether current call stack  was modified after applying the changes.
        /// </summary>
        public bool? stackChanged
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

        /// <summary>
        /// Exception details if any.
        /// </summary>
        public Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }
    }
}