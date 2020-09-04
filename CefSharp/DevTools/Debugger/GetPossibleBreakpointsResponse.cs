// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetPossibleBreakpointsResponse
    /// </summary>
    public class GetPossibleBreakpointsResponse
    {
        /// <summary>
        /// List of the possible breakpoint locations.
        /// </summary>
        public System.Collections.Generic.IList<BreakLocation> locations
        {
            get;
            set;
        }
    }
}