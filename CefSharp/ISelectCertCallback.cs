// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
using System;
using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for fetching the selected certificate.
    /// </summary>
    public interface ISelectCertCallback : IDisposable
    {
        /// <summary>
        /// Return the selected certificate
        /// </summary>
        /// <param name="selectedCert">selected certificate</param>
        void Select(X509Certificate selectedCert);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
