// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a JavaScript function.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FunctionCoverage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// JavaScript function name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("functionName"), IsRequired = (true))]
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Source ranges inside the function with coverage data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ranges"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Profiler.CoverageRange> Ranges
        {
            get;
            set;
        }

        /// <summary>
        /// Whether coverage data for this function has block granularity.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isBlockCoverage"), IsRequired = (true))]
        public bool IsBlockCoverage
        {
            get;
            set;
        }
    }
}