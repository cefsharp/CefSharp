// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// The list of profile nodes. First item is the root node.
        /// </summary>
        public System.Collections.Generic.IList<ProfileNode> Nodes
        {
            get;
            set;
        }

        /// <summary>
        /// Profiling start timestamp in microseconds.
        /// </summary>
        public long StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Profiling end timestamp in microseconds.
        /// </summary>
        public long EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Ids of samples top nodes.
        /// </summary>
        public int Samples
        {
            get;
            set;
        }

        /// <summary>
        /// Time intervals between adjacent samples in microseconds. The first delta is relative to the
        public int TimeDeltas
        {
            get;
            set;
        }
    }
}