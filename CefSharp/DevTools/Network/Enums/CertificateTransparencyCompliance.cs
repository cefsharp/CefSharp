// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Whether the request complied with Certificate Transparency policy.
    /// </summary>
    public enum CertificateTransparencyCompliance
    {
        /// <summary>
        /// unknown
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("unknown"))]
        Unknown,
        /// <summary>
        /// not-compliant
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("not-compliant"))]
        NotCompliant,
        /// <summary>
        /// compliant
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("compliant"))]
        Compliant
    }
}