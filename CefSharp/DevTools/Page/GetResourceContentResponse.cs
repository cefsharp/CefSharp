// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetResourceContentResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetResourceContentResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string content
        {
            get;
            set;
        }

        /// <summary>
        /// content
        /// </summary>
        public string Content
        {
            get
            {
                return content;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool base64Encoded
        {
            get;
            set;
        }

        /// <summary>
        /// base64Encoded
        /// </summary>
        public bool Base64Encoded
        {
            get
            {
                return base64Encoded;
            }
        }
    }
}