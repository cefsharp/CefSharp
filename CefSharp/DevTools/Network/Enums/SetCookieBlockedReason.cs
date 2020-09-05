// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Types of reasons why a cookie may not be stored from a response.
    /// </summary>
    public enum SetCookieBlockedReason
    {
        /// <summary>
        /// SecureOnly
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SecureOnly"))]
        SecureOnly,
        /// <summary>
        /// SameSiteStrict
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameSiteStrict"))]
        SameSiteStrict,
        /// <summary>
        /// SameSiteLax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameSiteLax"))]
        SameSiteLax,
        /// <summary>
        /// SameSiteUnspecifiedTreatedAsLax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameSiteUnspecifiedTreatedAsLax"))]
        SameSiteUnspecifiedTreatedAsLax,
        /// <summary>
        /// SameSiteNoneInsecure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameSiteNoneInsecure"))]
        SameSiteNoneInsecure,
        /// <summary>
        /// UserPreferences
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("UserPreferences"))]
        UserPreferences,
        /// <summary>
        /// SyntaxError
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SyntaxError"))]
        SyntaxError,
        /// <summary>
        /// SchemeNotSupported
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SchemeNotSupported"))]
        SchemeNotSupported,
        /// <summary>
        /// OverwriteSecure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("OverwriteSecure"))]
        OverwriteSecure,
        /// <summary>
        /// InvalidDomain
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("InvalidDomain"))]
        InvalidDomain,
        /// <summary>
        /// InvalidPrefix
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("InvalidPrefix"))]
        InvalidPrefix,
        /// <summary>
        /// UnknownError
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("UnknownError"))]
        UnknownError
    }
}