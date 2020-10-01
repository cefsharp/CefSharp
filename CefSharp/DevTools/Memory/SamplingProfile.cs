// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    /// <summary>
    /// Array of heap profile samples.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SamplingProfile : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Samples
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("samples"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Memory.SamplingProfileNode> Samples
        {
            get;
            set;
        }

        /// <summary>
        /// Modules
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("modules"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Memory.Module> Modules
        {
            get;
            set;
        }
    }
}