// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a JavaScript function.
    /// </summary>
    public class FunctionCoverage
    {
        /// <summary>
        /// JavaScript function name.
        /// </summary>
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Source ranges inside the function with coverage data.
        /// </summary>
        public System.Collections.Generic.IList<CoverageRange> Ranges
        {
            get;
            set;
        }

        /// <summary>
        /// Whether coverage data for this function has block granularity.
        /// </summary>
        public bool IsBlockCoverage
        {
            get;
            set;
        }
    }
}