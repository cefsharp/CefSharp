// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Details about the security state of the page certificate.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CertificateSecurityState : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Protocol name (e.g. "TLS 1.2" or "QUIC").
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("protocol"), IsRequired = (true))]
        public string Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Key Exchange used by the connection, or the empty string if not applicable.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyExchange"), IsRequired = (true))]
        public string KeyExchange
        {
            get;
            set;
        }

        /// <summary>
        /// (EC)DH group used by the connection, if applicable.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyExchangeGroup"), IsRequired = (false))]
        public string KeyExchangeGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Cipher name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cipher"), IsRequired = (true))]
        public string Cipher
        {
            get;
            set;
        }

        /// <summary>
        /// TLS MAC. Note that AEAD ciphers do not have separate MACs.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mac"), IsRequired = (false))]
        public string Mac
        {
            get;
            set;
        }

        /// <summary>
        /// Page certificate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificate"), IsRequired = (true))]
        public string[] Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate subject name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subjectName"), IsRequired = (true))]
        public string SubjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the issuing CA.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("issuer"), IsRequired = (true))]
        public string Issuer
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate valid from date.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("validFrom"), IsRequired = (true))]
        public long ValidFrom
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate valid to (expiration) date
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("validTo"), IsRequired = (true))]
        public long ValidTo
        {
            get;
            set;
        }

        /// <summary>
        /// The highest priority network error code, if the certificate has an error.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificateNetworkError"), IsRequired = (false))]
        public string CertificateNetworkError
        {
            get;
            set;
        }

        /// <summary>
        /// True if the certificate uses a weak signature aglorithm.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificateHasWeakSignature"), IsRequired = (true))]
        public bool CertificateHasWeakSignature
        {
            get;
            set;
        }

        /// <summary>
        /// True if the certificate has a SHA1 signature in the chain.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificateHasSha1Signature"), IsRequired = (true))]
        public bool CertificateHasSha1Signature
        {
            get;
            set;
        }

        /// <summary>
        /// True if modern SSL
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("modernSSL"), IsRequired = (true))]
        public bool ModernSSL
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL protocol.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("obsoleteSslProtocol"), IsRequired = (true))]
        public bool ObsoleteSslProtocol
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL key exchange.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("obsoleteSslKeyExchange"), IsRequired = (true))]
        public bool ObsoleteSslKeyExchange
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL cipher.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("obsoleteSslCipher"), IsRequired = (true))]
        public bool ObsoleteSslCipher
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL signature.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("obsoleteSslSignature"), IsRequired = (true))]
        public bool ObsoleteSslSignature
        {
            get;
            set;
        }
    }
}