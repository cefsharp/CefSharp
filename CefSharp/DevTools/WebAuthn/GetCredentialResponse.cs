// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// GetCredentialResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetCredentialResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.WebAuthn.Credential credential
        {
            get;
            set;
        }

        /// <summary>
        /// credential
        /// </summary>
        public CefSharp.DevTools.WebAuthn.Credential Credential
        {
            get
            {
                return credential;
            }
        }
    }
}