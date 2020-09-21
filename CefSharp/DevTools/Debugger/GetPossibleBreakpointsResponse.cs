// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetPossibleBreakpointsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPossibleBreakpointsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Debugger.BreakLocation> locations
        {
            get;
            set;
        }

        /// <summary>
        /// locations
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Debugger.BreakLocation> Locations
        {
            get
            {
                return locations;
            }
        }
    }
}