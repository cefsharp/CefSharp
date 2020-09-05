// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profile node. Holds callsite information, execution statistics and child nodes.
    /// </summary>
    public class ProfileNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Unique id of the node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Function location.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callFrame"), IsRequired = (true))]
        public Runtime.CallFrame CallFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Number of samples where this node was on top of the call stack.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hitCount"), IsRequired = (false))]
        public int? HitCount
        {
            get;
            set;
        }

        /// <summary>
        /// Child node ids.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("children"), IsRequired = (false))]
        public int? Children
        {
            get;
            set;
        }

        /// <summary>
        /// The reason of being not optimized. The function may be deoptimized or marked as don't
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("deoptReason"), IsRequired = (false))]
        public string DeoptReason
        {
            get;
            set;
        }

        /// <summary>
        /// An array of source position ticks.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("positionTicks"), IsRequired = (false))]
        public System.Collections.Generic.IList<PositionTickInfo> PositionTicks
        {
            get;
            set;
        }
    }
}