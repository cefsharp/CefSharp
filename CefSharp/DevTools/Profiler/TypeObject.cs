// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Describes a type collected during runtime.
    /// </summary>
    public class TypeObject
    {
        /// <summary>
        /// Name of a type collected with type profiling.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}