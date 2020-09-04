// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetBreakpointByUrlResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetBreakpointByUrlResponse
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
        internal System.Collections.Generic.IList<Location> locations
        {
            get;
            set;
        }

        /// <summary>
        /// List of the locations this breakpoint resolved into upon addition.
        /// </summary>
        public System.Collections.Generic.IList<Location> Locations
        {
            get
            {
                return locations;
            }
        }
    }
}