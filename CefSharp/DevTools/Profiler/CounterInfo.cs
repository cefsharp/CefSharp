// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Collected counter information.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CounterInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Counter name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Counter value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public int Value
        {
            get;
            set;
        }
    }
}