// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether the frame is a secure context and why it is the case.
    /// </summary>
    public enum SecureContextType
    {
        /// <summary>
        /// Secure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Secure"))]
        Secure,
        /// <summary>
        /// SecureLocalhost
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SecureLocalhost"))]
        SecureLocalhost,
        /// <summary>
        /// InsecureScheme
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("InsecureScheme"))]
        InsecureScheme,
        /// <summary>
        /// InsecureAncestor
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("InsecureAncestor"))]
        InsecureAncestor
    }
}