// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a JavaScript script.
    /// </summary>
    public class ScriptCoverage
    {
        /// <summary>
        /// JavaScript script id.
        /// </summary>
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Functions contained in the script that has coverage data.
        /// </summary>
        public System.Collections.Generic.IList<FunctionCoverage> Functions
        {
            get;
            set;
        }
    }
}