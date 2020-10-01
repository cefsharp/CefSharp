// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IO
{
    /// <summary>
    /// ResolveBlobResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ResolveBlobResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string uuid
        {
            get;
            set;
        }

        /// <summary>
        /// uuid
        /// </summary>
        public string Uuid
        {
            get
            {
                return uuid;
            }
        }
    }
}