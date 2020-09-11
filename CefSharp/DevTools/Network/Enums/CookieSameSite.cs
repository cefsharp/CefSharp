// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Represents the cookie's 'SameSite' status:
    /// https://tools.ietf.org/html/draft-west-first-party-cookies
    /// </summary>
    public enum CookieSameSite
    {
        /// <summary>
        /// Strict
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Strict"))]
        Strict,
        /// <summary>
        /// Lax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Lax"))]
        Lax,
        /// <summary>
        /// None
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("None"))]
        None
    }
}