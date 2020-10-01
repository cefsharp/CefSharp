// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange signature.
    /// https://wicg.github.io/webpackage/draft-yasskin-httpbis-origin-signed-exchanges-impl.html#rfc.section.3.1
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SignedExchangeSignature : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Signed exchange signature label.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("label"), IsRequired = (true))]
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// The hex string of signed exchange signature.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signature"), IsRequired = (true))]
        public string Signature
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature integrity.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("integrity"), IsRequired = (true))]
        public string Integrity
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature cert Url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certUrl"), IsRequired = (false))]
        public string CertUrl
        {
            get;
            set;
        }

        /// <summary>
        /// The hex string of signed exchange signature cert sha256.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certSha256"), IsRequired = (false))]
        public string CertSha256
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature validity Url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("validityUrl"), IsRequired = (true))]
        public string ValidityUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature date.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("date"), IsRequired = (true))]
        public int Date
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature expires.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("expires"), IsRequired = (true))]
        public int Expires
        {
            get;
            set;
        }

        /// <summary>
        /// The encoded certificates.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificates"), IsRequired = (false))]
        public string[] Certificates
        {
            get;
            set;
        }
    }
}