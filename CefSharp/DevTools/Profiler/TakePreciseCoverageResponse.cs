// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// TakePreciseCoverageResponse
    /// </summary>
    public class TakePreciseCoverageResponse
    {
        /// <summary>
        /// Coverage data for the current isolate.
        /// </summary>
        public System.Collections.Generic.IList<ScriptCoverage> result
        {
            get;
            set;
        }

        /// <summary>
        /// Monotonically increasing time (in seconds) when the coverage update was taken in the backend.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }
    }
}