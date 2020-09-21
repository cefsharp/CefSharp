// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// GetCertificateResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetCertificateResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] tableNames
        {
            get;
            set;
        }

        /// <summary>
        /// tableNames
        /// </summary>
        public string[] TableNames
        {
            get
            {
                return tableNames;
            }
        }
    }
}