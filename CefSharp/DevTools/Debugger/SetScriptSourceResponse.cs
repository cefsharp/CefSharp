// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetScriptSourceResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetScriptSourceResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CallFrame> callFrames
        {
            get;
            set;
        }

        /// <summary>
        /// New stack trace in case editing has happened while VM was stopped.
        /// </summary>
        public System.Collections.Generic.IList<CallFrame> CallFrames
        {
            get
            {
                return callFrames;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool? stackChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Whether current call stack  was modified after applying the changes.
        /// </summary>
        public bool? StackChanged
        {
            get
            {
                return stackChanged;
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

        [System.Runtime.Serialization.DataMemberAttribute]
        internal Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details if any.
        /// </summary>
        public Runtime.ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}