// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Type profile data collected during runtime for a JavaScript script.
    /// </summary>
    public class ScriptTypeProfile
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
        /// Type profile entries for parameters and return values of the functions in the script.
        /// </summary>
        public System.Collections.Generic.IList<TypeProfileEntry> Entries
        {
            get;
            set;
        }
    }
}