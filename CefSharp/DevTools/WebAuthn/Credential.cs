// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// Credential
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Credential : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// CredentialId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("credentialId"), IsRequired = (true))]
        public byte[] CredentialId
        {
            get;
            set;
        }

        /// <summary>
        /// IsResidentCredential
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isResidentCredential"), IsRequired = (true))]
        public bool IsResidentCredential
        {
            get;
            set;
        }

        /// <summary>
        /// Relying Party ID the credential is scoped to. Must be set when adding a
        /// credential.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rpId"), IsRequired = (false))]
        public string RpId
        {
            get;
            set;
        }

        /// <summary>
        /// The ECDSA P-256 private key in PKCS#8 format.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("privateKey"), IsRequired = (true))]
        public byte[] PrivateKey
        {
            get;
            set;
        }

        /// <summary>
        /// An opaque byte sequence with a maximum size of 64 bytes mapping the
        /// credential to a specific user.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("userHandle"), IsRequired = (false))]
        public byte[] UserHandle
        {
            get;
            set;
        }

        /// <summary>
        /// Signature counter. This is incremented by one for each successful
        /// assertion.
        /// See https://w3c.github.io/webauthn/#signature-counter
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signCount"), IsRequired = (true))]
        public int SignCount
        {
            get;
            set;
        }
    }
}