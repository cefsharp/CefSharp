// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Lists the errors that can be reported during Widevine Content Decryption Module (CDM) registration.
    /// </summary>
    public enum CdmRegistrationErrorCode
    {
        /// <summary>
        /// No error. Registration completed successfully.
        /// </summary>
        None,

        /// <summary>
        /// Required files or manifest contents are missing.
        /// </summary>
        IncorrectContents,

        /// <summary>
        /// The CDM is incompatible with the current Chromium version.
        /// </summary>
        Incompatible,

        /// <summary>
        /// CDM registration is not supported at this time.
        /// </summary>
        NotSupported,
    }
}
