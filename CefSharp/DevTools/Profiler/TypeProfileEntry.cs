// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Source offset and types for a parameter or return value.
    /// </summary>
    public class TypeProfileEntry
    {
        /// <summary>
        /// Source offset of the parameter or end of function for return values.
        /// </summary>
        public int Offset
        {
            get;
            set;
        }

        /// <summary>
        /// The types for this parameter or return value.
        /// </summary>
        public System.Collections.Generic.IList<TypeObject> Types
        {
            get;
            set;
        }
    }
}