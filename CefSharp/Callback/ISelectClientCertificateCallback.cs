// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used to select a client certificate for authentication.
    /// </summary>
    public interface ISelectClientCertificateCallback : IDisposable
    {
        /// <summary>
        /// Callback interface used to select a client certificate for authentication.
        /// </summary>
        /// <param name="selectedCert">selected certificate</param>
        void Select(X509Certificate2 selectedCert);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
