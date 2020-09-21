// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a JavaScript script.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ScriptCoverage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// JavaScript script id.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (true))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Functions contained in the script that has coverage data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("functions"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Profiler.FunctionCoverage> Functions
        {
            get;
            set;
        }
    }
}