// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Field type for a signed exchange related error.
    /// </summary>
    public enum SignedExchangeErrorField
    {
        /// <summary>
        /// signatureSig
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureSig"))]
        SignatureSig,
        /// <summary>
        /// signatureIntegrity
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureIntegrity"))]
        SignatureIntegrity,
        /// <summary>
        /// signatureCertUrl
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureCertUrl"))]
        SignatureCertUrl,
        /// <summary>
        /// signatureCertSha256
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureCertSha256"))]
        SignatureCertSha256,
        /// <summary>
        /// signatureValidityUrl
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureValidityUrl"))]
        SignatureValidityUrl,
        /// <summary>
        /// signatureTimestamps
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("signatureTimestamps"))]
        SignatureTimestamps
    }
}