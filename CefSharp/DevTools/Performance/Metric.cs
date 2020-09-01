// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Performance
{
    /// <summary>
    /// Run-time execution metric.
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Metric name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Metric value.
        /// </summary>
        public long Value
        {
            get;
            set;
        }
    }
}