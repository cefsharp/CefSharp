// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Use this static class to merge command line arguments.
    /// </summary>
    public sealed class CommandLineArgsMerger
    {
        /// <summary>
        /// Returns the merged command line features.
        /// </summary>
        /// <param name="existing">The existing features.</param>
        /// <param name="incoming">The features that should be merged.</param>
        /// <returns> the merged command line features. </returns>
        public static string MergeFeatures(string existing, string incoming)
        {
            var features = new List<string>();

            if (!string.IsNullOrWhiteSpace(existing))
            {
                foreach (var item in existing.Split(','))
                {
                    var trimmed = item.Trim();
                    if (!features.Contains(trimmed))
                    {
                        features.Add(trimmed);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(incoming))
            {
                foreach (var item in incoming.Split(','))
                {
                    var trimmed = item.Trim();
                    if (!features.Contains(trimmed))
                    {
                        features.Add(trimmed);
                    }
                }
            }

            return string.Join(",", features);
        }
    }
}
