// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Represents process info.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ProcessInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Specifies process type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies process id.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies cumulative CPU usage in seconds across all threads of the
        /// process since the process start.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cpuTime"), IsRequired = (true))]
        public long CpuTime
        {
            get;
            set;
        }
    }
}