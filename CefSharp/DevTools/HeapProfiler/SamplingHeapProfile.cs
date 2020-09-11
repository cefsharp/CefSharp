// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// Sampling profile.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SamplingHeapProfile : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Head
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("head"), IsRequired = (true))]
        public CefSharp.DevTools.HeapProfiler.SamplingHeapProfileNode Head
        {
            get;
            set;
        }

        /// <summary>
        /// Samples
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("samples"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.HeapProfiler.SamplingHeapProfileSample> Samples
        {
            get;
            set;
        }
    }
}