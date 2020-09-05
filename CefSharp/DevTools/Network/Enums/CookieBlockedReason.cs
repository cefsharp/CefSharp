// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Types of reasons why a cookie may not be sent with a request.
    /// </summary>
    public enum CookieBlockedReason
    {
        /// <summary>
        /// SecureOnly
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SecureOnly"))]
        SecureOnly,
        /// <summary>
        /// NotOnPath
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("NotOnPath"))]
        NotOnPath,
        /// <summary>
        /// DomainMismatch
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("DomainMismatch"))]
        DomainMismatch,
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
        /// UnknownError
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("UnknownError"))]
        UnknownError
    }
}