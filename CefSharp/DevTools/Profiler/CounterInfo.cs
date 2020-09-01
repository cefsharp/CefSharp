// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Collected counter information.
    /// </summary>
    public class CounterInfo
    {
        /// <summary>
        /// Counter name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Counter value.
        /// </summary>
        public int Value
        {
            get;
            set;
        }
    }
}