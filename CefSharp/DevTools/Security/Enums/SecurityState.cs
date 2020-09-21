// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// The security level of a page or resource.
    /// </summary>
    public enum SecurityState
    {
        /// <summary>
        /// unknown
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("unknown"))]
        Unknown,
        /// <summary>
        /// neutral
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("neutral"))]
        Neutral,
        /// <summary>
        /// insecure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("insecure"))]
        Insecure,
        /// <summary>
        /// secure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("secure"))]
        Secure,
        /// <summary>
        /// info
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("info"))]
        Info,
        /// <summary>
        /// insecure-broken
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("insecure-broken"))]
        InsecureBroken
    }
}