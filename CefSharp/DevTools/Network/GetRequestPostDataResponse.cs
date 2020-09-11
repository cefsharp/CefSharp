// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// GetRequestPostDataResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetRequestPostDataResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string postData
        {
            get;
            set;
        }

        /// <summary>
        /// postData
        /// </summary>
        public string PostData
        {
            get
            {
                return postData;
            }
        }
    }
}