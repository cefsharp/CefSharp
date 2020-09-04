// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profile node. Holds callsite information, execution statistics and child nodes.
    /// </summary>
    public class ProfileNode
    {
        /// <summary>
        /// Unique id of the node.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Function location.
        /// </summary>
        public Runtime.CallFrame CallFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Number of samples where this node was on top of the call stack.
        /// </summary>
        public int? HitCount
        {
            get;
            set;
        }

        /// <summary>
        /// Child node ids.
        /// </summary>
        public int? Children
        {
            get;
            set;
        }

        /// <summary>
        /// The reason of being not optimized. The function may be deoptimized or marked as don't
        public string DeoptReason
        {
            get;
            set;
        }

        /// <summary>
        /// An array of source position ticks.
        /// </summary>
        public System.Collections.Generic.IList<PositionTickInfo> PositionTicks
        {
            get;
            set;
        }
    }
}