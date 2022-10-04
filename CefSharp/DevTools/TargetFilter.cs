// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// The entries in TargetFilter are matched sequentially against targets and the first entry that matches
    /// determines if the target is included or not, depending on the value of exclude field in the entry.
    /// If filter is not specified, the one assumed is [{type: "browser", exclude: true}, {type: "tab", exclude: true}, {}] (i.e. include everything but browser and tab).
    /// </summary>
    public class TargetFilter : DevToolsDomainEntityBase
    {
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Exclude
        /// </summary>
        public bool Exclude { get; set; }
    }
}
