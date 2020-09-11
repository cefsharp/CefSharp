// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// CompositingReasonsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CompositingReasonsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] compositingReasons
        {
            get;
            set;
        }

        /// <summary>
        /// compositingReasons
        /// </summary>
        public string[] CompositingReasons
        {
            get
            {
                return compositingReasons;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] compositingReasonIds
        {
            get;
            set;
        }

        /// <summary>
        /// compositingReasonIds
        /// </summary>
        public string[] CompositingReasonIds
        {
            get
            {
                return compositingReasonIds;
            }
        }
    }
}