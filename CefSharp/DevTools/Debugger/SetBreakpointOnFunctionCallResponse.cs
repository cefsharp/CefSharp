// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetBreakpointOnFunctionCallResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetBreakpointOnFunctionCallResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string breakpointId
        {
            get;
            set;
        }

        /// <summary>
        /// breakpointId
        /// </summary>
        public string BreakpointId
        {
            get
            {
                return breakpointId;
            }
        }
    }
}