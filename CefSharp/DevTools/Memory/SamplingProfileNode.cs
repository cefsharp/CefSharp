// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    /// <summary>
    /// Heap profile sample.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SamplingProfileNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Size of the sampled allocation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("size"), IsRequired = (true))]
        public long Size
        {
            get;
            set;
        }

        /// <summary>
        /// Total bytes attributed to this sample.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("total"), IsRequired = (true))]
        public long Total
        {
            get;
            set;
        }

        /// <summary>
        /// Execution stack at the point of allocation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stack"), IsRequired = (true))]
        public string[] Stack
        {
            get;
            set;
        }
    }
}