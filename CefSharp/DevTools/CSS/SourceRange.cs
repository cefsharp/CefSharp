// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Text range within a resource. All numbers are zero-based.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SourceRange : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Start line of range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startLine"), IsRequired = (true))]
        public int StartLine
        {
            get;
            set;
        }

        /// <summary>
        /// Start column of range (inclusive).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startColumn"), IsRequired = (true))]
        public int StartColumn
        {
            get;
            set;
        }

        /// <summary>
        /// End line of range
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endLine"), IsRequired = (true))]
        public int EndLine
        {
            get;
            set;
        }

        /// <summary>
        /// End column of range (exclusive).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endColumn"), IsRequired = (true))]
        public int EndColumn
        {
            get;
            set;
        }
    }
}