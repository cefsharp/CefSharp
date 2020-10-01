// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Network level fetch failure reason.
    /// </summary>
    public enum ErrorReason
    {
        /// <summary>
        /// Failed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Failed"))]
        Failed,
        /// <summary>
        /// Aborted
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Aborted"))]
        Aborted,
        /// <summary>
        /// TimedOut
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("TimedOut"))]
        TimedOut,
        /// <summary>
        /// AccessDenied
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("AccessDenied"))]
        AccessDenied,
        /// <summary>
        /// ConnectionClosed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ConnectionClosed"))]
        ConnectionClosed,
        /// <summary>
        /// ConnectionReset
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ConnectionReset"))]
        ConnectionReset,
        /// <summary>
        /// ConnectionRefused
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ConnectionRefused"))]
        ConnectionRefused,
        /// <summary>
        /// ConnectionAborted
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ConnectionAborted"))]
        ConnectionAborted,
        /// <summary>
        /// ConnectionFailed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ConnectionFailed"))]
        ConnectionFailed,
        /// <summary>
        /// NameNotResolved
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("NameNotResolved"))]
        NameNotResolved,
        /// <summary>
        /// InternetDisconnected
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("InternetDisconnected"))]
        InternetDisconnected,
        /// <summary>
        /// AddressUnreachable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("AddressUnreachable"))]
        AddressUnreachable,
        /// <summary>
        /// BlockedByClient
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("BlockedByClient"))]
        BlockedByClient,
        /// <summary>
        /// BlockedByResponse
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("BlockedByResponse"))]
        BlockedByResponse
    }
}