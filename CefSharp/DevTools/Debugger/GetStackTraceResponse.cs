// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetStackTraceResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetStackTraceResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.StackTrace stackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// stackTrace
        /// </summary>
        public CefSharp.DevTools.Runtime.StackTrace StackTrace
        {
            get
            {
                return stackTrace;
            }
        }
    }
}