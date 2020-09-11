// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetScriptSourceResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetScriptSourceResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Debugger.CallFrame> callFrames
        {
            get;
            set;
        }

        /// <summary>
        /// callFrames
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Debugger.CallFrame> CallFrames
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
        /// stackChanged
        /// </summary>
        public bool? StackChanged
        {
            get
            {
                return stackChanged;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.StackTrace asyncStackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// asyncStackTrace
        /// </summary>
        public CefSharp.DevTools.Runtime.StackTrace AsyncStackTrace
        {
            get
            {
                return asyncStackTrace;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.StackTraceId asyncStackTraceId
        {
            get;
            set;
        }

        /// <summary>
        /// asyncStackTraceId
        /// </summary>
        public CefSharp.DevTools.Runtime.StackTraceId AsyncStackTraceId
        {
            get
            {
                return asyncStackTraceId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// exceptionDetails
        /// </summary>
        public CefSharp.DevTools.Runtime.ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}