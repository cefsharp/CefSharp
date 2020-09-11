// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// AddVirtualAuthenticatorResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AddVirtualAuthenticatorResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string authenticatorId
        {
            get;
            set;
        }

        /// <summary>
        /// authenticatorId
        /// </summary>
        public string AuthenticatorId
        {
            get
            {
                return authenticatorId;
            }
        }
    }
}