// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Search match for resource.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SearchMatch : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Line number in resource content.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public long LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Line with match content.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineContent"), IsRequired = (true))]
        public string LineContent
        {
            get;
            set;
        }
    }
}