// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// PerformSearchResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PerformSearchResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string searchId
        {
            get;
            set;
        }

        /// <summary>
        /// searchId
        /// </summary>
        public string SearchId
        {
            get
            {
                return searchId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int resultCount
        {
            get;
            set;
        }

        /// <summary>
        /// resultCount
        /// </summary>
        public int ResultCount
        {
            get
            {
                return resultCount;
            }
        }
    }
}