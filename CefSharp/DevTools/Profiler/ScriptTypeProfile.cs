// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Type profile data collected during runtime for a JavaScript script.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ScriptTypeProfile : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// Type profile entries for parameters and return values of the functions in the script.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("entries"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Profiler.TypeProfileEntry> Entries
        {
            get;
            set;
        }
    }
}