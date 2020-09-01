// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Chrome histogram.
    /// </summary>
    public class Histogram
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Sum of sample values.
        /// </summary>
        public int Sum
        {
            get;
            set;
        }

        /// <summary>
        /// Total number of samples.
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Buckets.
        /// </summary>
        public System.Collections.Generic.IList<Bucket> Buckets
        {
            get;
            set;
        }
    }
}