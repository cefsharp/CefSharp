// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetSearchResultsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetSearchResultsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int[] nodeIds
        {
            get;
            set;
        }

        /// <summary>
        /// nodeIds
        /// </summary>
        public int[] NodeIds
        {
            get
            {
                return nodeIds;
            }
        }
    }
}