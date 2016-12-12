// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Class representing the SSL information for a navigation entry.
    /// </summary>
    public struct SslStatus
    {
        private bool isSecureConnection;
        private CertStatus certStatus;
        private SslVersion sslVersion;
        private SslContentStatus contentStatus;
        private X509Certificate2 certificate;

        public SslStatus(bool isSecureConnection, CertStatus certStatus, SslVersion sslVersion, SslContentStatus contentStatus, X509Certificate2 certificate)
        {
            this.isSecureConnection = isSecureConnection;
            this.certStatus = certStatus;
            this.sslVersion = sslVersion;
            this.contentStatus = contentStatus;
            this.certificate = certificate;
        }
        
        /// <summary>
        /// Returns true if the status is related to a secure SSL/TLS connection.
        /// </summary>
        public bool IsSecureConnection
        {
            get { return isSecureConnection; }
        }

        /// <summary>
        /// Returns a bitmask containing any and all problems verifying the server
        /// certificate.
        /// </summary>
        /// <returns></returns>
        public CertStatus CertStatus
        {
            get { return certStatus; }
        }

        /// <summary>
        /// Returns the SSL version used for the SSL connection.
        /// </summary>
        /// <returns></returns>
        public SslVersion SslVersion
        {
            get { return sslVersion; }
        }

        ///
        // Returns a bitmask containing the page security content status.
        ///
        public SslContentStatus ContentStatus
        {
            get { return contentStatus; }
        }

        ///
        /// Returns the X.509 certificate.
        ///
        public X509Certificate2 X509Certificate
        {
            get { return certificate; }
        }
    }
}
