// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Security details about a request.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SecurityDetails : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// Certificate ID value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificateId"), IsRequired = (true))]
        public int CertificateId
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
        /// Subject Alternative Name (SAN) DNS names and IP addresses.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sanList"), IsRequired = (true))]
        public string[] SanList
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
        /// List of signed certificate timestamps (SCTs).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signedCertificateTimestampList"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Network.SignedCertificateTimestamp> SignedCertificateTimestampList
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the request complied with Certificate Transparency policy
        /// </summary>
        public CefSharp.DevTools.Network.CertificateTransparencyCompliance CertificateTransparencyCompliance
        {
            get
            {
                return (CefSharp.DevTools.Network.CertificateTransparencyCompliance)(StringToEnum(typeof(CefSharp.DevTools.Network.CertificateTransparencyCompliance), certificateTransparencyCompliance));
            }

            set
            {
                certificateTransparencyCompliance = (EnumToString(value));
            }
        }

        /// <summary>
        /// Whether the request complied with Certificate Transparency policy
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificateTransparencyCompliance"), IsRequired = (true))]
        internal string certificateTransparencyCompliance
        {
            get;
            set;
        }
    }
}