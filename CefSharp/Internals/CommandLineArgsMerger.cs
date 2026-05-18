// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;

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
            var features = new HashSet<string>();

            AddFeatures(features, existing);
            AddFeatures(features, incoming);

            return string.Join(",", features.OrderBy(i => i));
        }

        private static void AddFeatures(HashSet<string> features, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            foreach (var item in value.Split(','))
            {
                features.Add(item.Trim());
            }
        }
    }
}
