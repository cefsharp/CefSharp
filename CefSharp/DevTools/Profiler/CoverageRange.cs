// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Coverage data for a source range.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CoverageRange : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// JavaScript script source offset for the range start.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startOffset"), IsRequired = (true))]
        public int StartOffset
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script source offset for the range end.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endOffset"), IsRequired = (true))]
        public int EndOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Collected execution count of the source range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("count"), IsRequired = (true))]
        public int Count
        {
            get;
            set;
        }
    }
}