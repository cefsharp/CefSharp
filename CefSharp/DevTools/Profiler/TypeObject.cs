// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Describes a type collected during runtime.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class TypeObject : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name of a type collected with type profiling.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }
    }
}