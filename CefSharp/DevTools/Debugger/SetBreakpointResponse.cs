// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetBreakpointResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetBreakpointResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string breakpointId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the created breakpoint for further reference.
        /// </summary>
        public string BreakpointId
        {
            get
            {
                return breakpointId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Debugger.Location actualLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location this breakpoint resolved into.
        /// </summary>
        public CefSharp.DevTools.Debugger.Location ActualLocation
        {
            get
            {
                return actualLocation;
            }
        }
    }
}