// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// AuthenticatorTransport
    /// </summary>
    public enum AuthenticatorTransport
    {
        /// <summary>
        /// usb
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("usb"))]
        Usb,
        /// <summary>
        /// nfc
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("nfc"))]
        Nfc,
        /// <summary>
        /// ble
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ble"))]
        Ble,
        /// <summary>
        /// cable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cable"))]
        Cable,
        /// <summary>
        /// internal
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("internal"))]
        Internal
    }
}