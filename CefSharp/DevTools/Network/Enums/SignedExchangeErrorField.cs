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
        SignatureSig,
        SignatureIntegrity,
        SignatureCertUrl,
        SignatureCertSha256,
        SignatureValidityUrl,
        SignatureTimestamps
    }
}