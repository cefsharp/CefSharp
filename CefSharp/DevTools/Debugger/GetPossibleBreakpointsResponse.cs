// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetPossibleBreakpointsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPossibleBreakpointsResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<BreakLocation> locations
        {
            get;
            set;
        }

        /// <summary>
        /// List of the possible breakpoint locations.
        /// </summary>
        public System.Collections.Generic.IList<BreakLocation> Locations
        {
            get
            {
                return locations;
            }
        }
    }
}