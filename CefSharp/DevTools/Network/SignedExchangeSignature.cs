// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange signature.
    public class SignedExchangeSignature
    {
        /// <summary>
        /// Signed exchange signature label.
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// The hex string of signed exchange signature.
        /// </summary>
        public string Signature
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature integrity.
        /// </summary>
        public string Integrity
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature cert Url.
        /// </summary>
        public string CertUrl
        {
            get;
            set;
        }

        /// <summary>
        /// The hex string of signed exchange signature cert sha256.
        /// </summary>
        public string CertSha256
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature validity Url.
        /// </summary>
        public string ValidityUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature date.
        /// </summary>
        public int Date
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange signature expires.
        /// </summary>
        public int Expires
        {
            get;
            set;
        }

        /// <summary>
        /// The encoded certificates.
        /// </summary>
        public string Certificates
        {
            get;
            set;
        }
    }
}