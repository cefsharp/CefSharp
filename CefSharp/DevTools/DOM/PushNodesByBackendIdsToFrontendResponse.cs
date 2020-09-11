// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// PushNodesByBackendIdsToFrontendResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PushNodesByBackendIdsToFrontendResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int[] nodeIds
        {
            get;
            set;
        }

        /// <summary>
        /// The array of ids of pushed nodes that correspond to the backend ids specified in
        public int[] NodeIds
        {
            get
            {
                return nodeIds;
            }
        }
    }
}