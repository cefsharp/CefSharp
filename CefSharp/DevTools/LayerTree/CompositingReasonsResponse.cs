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
        /// A list of strings specifying reasons for the given layer to become composited.
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
        /// A list of strings specifying reason IDs for the given layer to become composited.
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