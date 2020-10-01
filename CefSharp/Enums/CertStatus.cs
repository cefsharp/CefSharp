// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    ///
    /// Supported certificate status code values. See net\cert\cert_status_flags.h
    /// for more information. CERT_STATUS_NONE is new in CEF because we use an
    /// enum while cert_status_flags.h uses a typedef and static const variables.
    /// </summary>
    [Flags]
    public enum CertStatus
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// CommonNameInvalid
        /// </summary>
        CommonNameInvalid = 1 << 0,
        /// <summary>
        /// DateInvalid
        /// </summary>
        DateInvalid = 1 << 1,
        /// <summary>
        /// AuthorityInvalid
        /// </summary>
        AuthorityInvalid = 1 << 2,
        // 1 << 3 is reserved for ERR_CERT_CONTAINS_ERRORS (not useful with WinHTTP).
        /// <summary>
        /// NoRevocation_Mechanism
        /// </summary>
        NoRevocation_Mechanism = 1 << 4,
        /// <summary>
        /// UnableToCheckRevocation
        /// </summary>
        UnableToCheckRevocation = 1 << 5,
        /// <summary>
        /// Revoked
        /// </summary>
        Revoked = 1 << 6,
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = 1 << 7,
        /// <summary>
        /// WeakSignatureAlgorithm
        /// </summary>
        WeakSignatureAlgorithm = 1 << 8,
        // 1 << 9 was used for CERT_STATUS_NOT_IN_DNS
        /// <summary>
        /// NonUniqueName
        /// </summary>
        NonUniqueName = 1 << 10,
        /// <summary>
        /// WeakKey
        /// </summary>
        WeakKey = 1 << 11,
        // 1 << 12 was used for CERT_STATUS_WEAK_DH_KEY
        /// <summary>
        /// PinnedKeyMissing
        /// </summary>
        PinnedKeyMissing = 1 << 13,
        /// <summary>
        /// NameConstraintViolation
        /// </summary>
        NameConstraintViolation = 1 << 14,
        /// <summary>
        /// ValidityTooLong
        /// </summary>
        ValidityTooLong = 1 << 15,

        // Bits 16 to 31 are for non-error statuses.
        /// <summary>
        /// IsEv
        /// </summary>
        IsEv = 1 << 16,
        /// <summary>
        /// RevCheckingEnabled
        /// </summary>
        RevCheckingEnabled = 1 << 17,
        // Bit 18 was CERT_STATUS_IS_DNSSEC
        /// <summary>
        /// Sha1SignaturePresent
        /// </summary>
        Sha1SignaturePresent = 1 << 19,
        /// <summary>
        /// CtComplianceFailed
        /// </summary>
        CtComplianceFailed = 1 << 20
    }
}
