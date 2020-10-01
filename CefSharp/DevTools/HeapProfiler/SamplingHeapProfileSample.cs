// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// A single sample from a sampling profile.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SamplingHeapProfileSample : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Allocation size in bytes attributed to the sample.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("size"), IsRequired = (true))]
        public long Size
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the corresponding profile tree node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeId"), IsRequired = (true))]
        public int NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Time-ordered sample ordinal number. It is unique across all profiles retrieved
        /// between startSampling and stopSampling.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ordinal"), IsRequired = (true))]
        public long Ordinal
        {
            get;
            set;
        }
    }
}