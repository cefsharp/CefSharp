// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// Sampling Heap Profile node. Holds callsite information, allocation statistics and child nodes.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SamplingHeapProfileNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Function location.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callFrame"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.CallFrame CallFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Allocations size in bytes for the node excluding children.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("selfSize"), IsRequired = (true))]
        public long SelfSize
        {
            get;
            set;
        }

        /// <summary>
        /// Node id. Ids are unique across all profiles collected between startSampling and stopSampling.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Child nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("children"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.HeapProfiler.SamplingHeapProfileNode> Children
        {
            get;
            set;
        }
    }
}