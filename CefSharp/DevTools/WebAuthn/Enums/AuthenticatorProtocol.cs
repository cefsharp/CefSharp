// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// AuthenticatorProtocol
    /// </summary>
    public enum AuthenticatorProtocol
    {
        /// <summary>
        /// u2f
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("u2f"))]
        U2f,
        /// <summary>
        /// ctap2
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ctap2"))]
        Ctap2
    }
}