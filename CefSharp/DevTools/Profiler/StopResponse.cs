// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// StopResponse
    /// </summary>
    public class StopResponse
    {
        /// <summary>
        /// Recorded profile.
        /// </summary>
        public Profile profile
        {
            get;
            set;
        }
    }
}