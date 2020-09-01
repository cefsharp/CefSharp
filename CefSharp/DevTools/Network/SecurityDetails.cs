// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Security details about a request.
    /// </summary>
    public class SecurityDetails
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
        /// Certificate ID value.
        /// </summary>
        public int CertificateId
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
        /// Subject Alternative Name (SAN) DNS names and IP addresses.
        /// </summary>
        public string SanList
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
        /// List of signed certificate timestamps (SCTs).
        /// </summary>
        public System.Collections.Generic.IList<SignedCertificateTimestamp> SignedCertificateTimestampList
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the request complied with Certificate Transparency policy
        /// </summary>
        public string CertificateTransparencyCompliance
        {
            get;
            set;
        }
    }
}