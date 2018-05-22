// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Class representing SSL information. 
    /// </summary>
    public interface ISslInfo
    {
        /// <summary>
        /// Returns a bitmask containing any and all problems verifying the server
        /// certificate.
        /// </summary>
        CertStatus CertStatus { get; }

        /// <summary>
        /// Returns the X.509 certificate.
        /// </summary>
        X509Certificate2 X509Certificate { get; }
    }
}
