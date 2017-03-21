// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Class representing the SSL information for a navigation entry.
    /// </summary>
    public sealed class SslStatus
    {
        /// <summary>
        /// Returns true if the status is related to a secure SSL/TLS connection.
        /// </summary>
        public bool IsSecureConnection { get; private set; }
        
        /// <summary>
        /// Returns a bitmask containing any and all problems verifying the server
        /// certificate.
        /// </summary>
        /// <returns></returns>
        public CertStatus CertStatus { get; private set; }

        /// <summary>
        /// Returns the SSL version used for the SSL connection.
        /// </summary>
        /// <returns></returns>
        public SslVersion SslVersion { get; private set; }

        /// <summary>
        /// Returns a bitmask containing the page security content status.
        /// </summary>
        public SslContentStatus ContentStatus { get; private set; }

        /// <summary>
        /// Returns the X.509 certificate.
        /// </summary>
        public X509Certificate2 X509Certificate { get; private set; }

        public SslStatus(bool isSecureConnection, CertStatus certStatus, SslVersion sslVersion, SslContentStatus contentStatus, X509Certificate2 certificate)
        {
            IsSecureConnection = isSecureConnection;
            CertStatus = certStatus;
            SslVersion = sslVersion;
            ContentStatus = contentStatus;
            X509Certificate = certificate;
        }
    }
}
