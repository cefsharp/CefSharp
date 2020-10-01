// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// GetCredentialsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetCredentialsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.WebAuthn.Credential> credentials
        {
            get;
            set;
        }

        /// <summary>
        /// credentials
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.WebAuthn.Credential> Credentials
        {
            get
            {
                return credentials;
            }
        }
    }
}