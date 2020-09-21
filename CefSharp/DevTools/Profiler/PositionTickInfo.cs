// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Specifies a number of samples attributed to a certain source position.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PositionTickInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Source line number (1-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("line"), IsRequired = (true))]
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Number of samples attributed to the source line.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ticks"), IsRequired = (true))]
        public int Ticks
        {
            get;
            set;
        }
    }
}