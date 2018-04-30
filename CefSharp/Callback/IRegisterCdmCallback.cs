// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Content Decryption Module (CDM) registration callback used for asynchronous completion.
    /// </summary>
    public interface IRegisterCdmCallback : IDisposable
    {
        /// <summary>
        /// Method that will be called once CDM registration is complete
        /// </summary>
        /// <param name="registration">The result of the CDM registration process</param>
        void OnRegistrationComplete(CdmRegistration registration);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
