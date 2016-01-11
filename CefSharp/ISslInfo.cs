// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface ISslInfo
    {
        /// <summary>
        /// Returns the subject of the X.509 certificate. For HTTPS server
        /// certificates this represents the web server.  The common name of the
        /// subject should match the host name of the web server.
        /// </summary>
        //ISslCertPrincipal Subject { get; }

        /// <summary>
        /// Returns the issuer of the X.509 certificate.
        /// </summary>
        //ISslCertPrincipal Issuer { get; }

        /// <summary>
        /// Returns the DER encoded serial number for the X.509 certificate. The value
        /// possibly includes a leading 00 byte.
        /// </summary>
        byte[] SerialNumber { get; }
  
        /// <summary>
        /// Returns the date before which the X.509 certificate is invalid.
        /// will return null if no date was specified.
        /// </summary>
        DateTime? ValidStart { get; }

        /// <summary>
        /// Returns the date after which the X.509 certificate is invalid.
        /// will return null if no date was specified.
        /// </summary>
        DateTime? ValidExpiry { get; }

        /// <summary>
        /// Returns the DER encoded data for the X.509 certificate.
        /// </summary>
        byte[] DerEncoded { get; }

        /// <summary>
        /// Returns the PEM encoded data for the X.509 certificate.
        /// </summary>
        byte[] PemEncoded { get; }
    }
}
