// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profile.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Profile : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The list of profile nodes. First item is the root node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodes"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Profiler.ProfileNode> Nodes
        {
            get;
            set;
        }

        /// <summary>
        /// Profiling start timestamp in microseconds.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startTime"), IsRequired = (true))]
        public long StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Profiling end timestamp in microseconds.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endTime"), IsRequired = (true))]
        public long EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Ids of samples top nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("samples"), IsRequired = (false))]
        public int[] Samples
        {
            get;
            set;
        }

        /// <summary>
        /// Time intervals between adjacent samples in microseconds. The first delta is relative to the
        /// profile startTime.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timeDeltas"), IsRequired = (false))]
        public int[] TimeDeltas
        {
            get;
            set;
        }
    }
}