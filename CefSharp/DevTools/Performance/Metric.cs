// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Performance
{
    /// <summary>
    /// Run-time execution metric.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Metric : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Metric name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Metric value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public long Value
        {
            get;
            set;
        }
    }
}