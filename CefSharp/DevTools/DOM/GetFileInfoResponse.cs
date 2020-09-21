// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetFileInfoResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetFileInfoResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string path
        {
            get;
            set;
        }

        /// <summary>
        /// path
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }
    }
}