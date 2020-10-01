// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Tracing
{
    /// <summary>
    /// TraceConfig
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class TraceConfig : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Controls how the trace buffer stores data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("recordMode"), IsRequired = (false))]
        public string RecordMode
        {
            get;
            set;
        }

        /// <summary>
        /// Turns on JavaScript stack sampling.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("enableSampling"), IsRequired = (false))]
        public bool? EnableSampling
        {
            get;
            set;
        }

        /// <summary>
        /// Turns on system tracing.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("enableSystrace"), IsRequired = (false))]
        public bool? EnableSystrace
        {
            get;
            set;
        }

        /// <summary>
        /// Turns on argument filter.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("enableArgumentFilter"), IsRequired = (false))]
        public bool? EnableArgumentFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Included category filters.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("includedCategories"), IsRequired = (false))]
        public string[] IncludedCategories
        {
            get;
            set;
        }

        /// <summary>
        /// Excluded category filters.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("excludedCategories"), IsRequired = (false))]
        public string[] ExcludedCategories
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration to synthesize the delays in tracing.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("syntheticDelays"), IsRequired = (false))]
        public string[] SyntheticDelays
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration for memory dump triggers. Used only when "memory-infra" category is enabled.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("memoryDumpConfig"), IsRequired = (false))]
        public CefSharp.DevTools.Tracing.MemoryDumpConfig MemoryDumpConfig
        {
            get;
            set;
        }
    }
}