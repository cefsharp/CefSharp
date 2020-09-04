// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// SetInstrumentationBreakpointResponse
    /// </summary>
    public class SetInstrumentationBreakpointResponse
    {
        /// <summary>
        /// Id of the created breakpoint for further reference.
        /// </summary>
        public string breakpointId
        {
            get;
            set;
        }
    }
}