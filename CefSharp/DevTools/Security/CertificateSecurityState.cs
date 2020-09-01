// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Details about the security state of the page certificate.
    /// </summary>
    public class CertificateSecurityState
    {
        /// <summary>
        /// Protocol name (e.g. "TLS 1.2" or "QUIC").
        /// </summary>
        public string Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Key Exchange used by the connection, or the empty string if not applicable.
        /// </summary>
        public string KeyExchange
        {
            get;
            set;
        }

        /// <summary>
        /// (EC)DH group used by the connection, if applicable.
        /// </summary>
        public string KeyExchangeGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Cipher name.
        /// </summary>
        public string Cipher
        {
            get;
            set;
        }

        /// <summary>
        /// TLS MAC. Note that AEAD ciphers do not have separate MACs.
        /// </summary>
        public string Mac
        {
            get;
            set;
        }

        /// <summary>
        /// Page certificate.
        /// </summary>
        public string Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate subject name.
        /// </summary>
        public string SubjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the issuing CA.
        /// </summary>
        public string Issuer
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate valid from date.
        /// </summary>
        public long ValidFrom
        {
            get;
            set;
        }

        /// <summary>
        /// Certificate valid to (expiration) date
        /// </summary>
        public long ValidTo
        {
            get;
            set;
        }

        /// <summary>
        /// The highest priority network error code, if the certificate has an error.
        /// </summary>
        public string CertificateNetworkError
        {
            get;
            set;
        }

        /// <summary>
        /// True if the certificate uses a weak signature aglorithm.
        /// </summary>
        public bool CertificateHasWeakSignature
        {
            get;
            set;
        }

        /// <summary>
        /// True if the certificate has a SHA1 signature in the chain.
        /// </summary>
        public bool CertificateHasSha1Signature
        {
            get;
            set;
        }

        /// <summary>
        /// True if modern SSL
        /// </summary>
        public bool ModernSSL
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL protocol.
        /// </summary>
        public bool ObsoleteSslProtocol
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL key exchange.
        /// </summary>
        public bool ObsoleteSslKeyExchange
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL cipher.
        /// </summary>
        public bool ObsoleteSslCipher
        {
            get;
            set;
        }

        /// <summary>
        /// True if the connection is using an obsolete SSL signature.
        /// </summary>
        public bool ObsoleteSslSignature
        {
            get;
            set;
        }
    }
}