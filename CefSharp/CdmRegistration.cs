// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Represents the response to an attempt to register the Widevine Content Decryption Module (CDM)
    /// </summary>
    public sealed class CdmRegistration
    {
        /// <summary>
        /// If CDM registration succeeded then value will be <see cref="CdmRegistrationErrorCode.None"/>, for other values see the enumeration <see cref="CdmRegistrationErrorCode" />.
        /// </summary>
        public CdmRegistrationErrorCode ErrorCode { get; private set; }
        
        /// <summary>
        /// Contains an error message containing additional information if <see cref="ErrorCode"/> is not <see cref="CdmRegistrationErrorCode.None"/>.
        /// </summary>
        public string ErrorMessage { get; private set; }

        public CdmRegistration(CdmRegistrationErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
