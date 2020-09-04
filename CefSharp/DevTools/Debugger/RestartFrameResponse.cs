// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// RestartFrameResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RestartFrameResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CallFrame> callFrames
        {
            get;
            set;
        }

        /// <summary>
        /// New stack trace.
        /// </summary>
        public System.Collections.Generic.IList<CallFrame> CallFrames
        {
            get
            {
                return callFrames;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal Runtime.StackTrace asyncStackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Async stack trace, if any.
        /// </summary>
        public Runtime.StackTrace AsyncStackTrace
        {
            get
            {
                return asyncStackTrace;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal Runtime.StackTraceId asyncStackTraceId
        {
            get;
            set;
        }

        /// <summary>
        /// Async stack trace, if any.
        /// </summary>
        public Runtime.StackTraceId AsyncStackTraceId
        {
            get
            {
                return asyncStackTraceId;
            }
        }
    }
}