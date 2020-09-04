// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// GetBestEffortCoverageResponse
    /// </summary>
    public class GetBestEffortCoverageResponse
    {
        /// <summary>
        /// Coverage data for the current isolate.
        /// </summary>
        public System.Collections.Generic.IList<ScriptCoverage> result
        {
            get;
            set;
        }
    }
}