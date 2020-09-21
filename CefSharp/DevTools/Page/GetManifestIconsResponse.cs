// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetManifestIconsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetManifestIconsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string primaryIcon
        {
            get;
            set;
        }

        /// <summary>
        /// primaryIcon
        /// </summary>
        public byte[] PrimaryIcon
        {
            get
            {
                return Convert(primaryIcon);
            }
        }
    }
}