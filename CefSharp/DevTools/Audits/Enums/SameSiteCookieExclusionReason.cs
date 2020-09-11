// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// SameSiteCookieExclusionReason
    /// </summary>
    public enum SameSiteCookieExclusionReason
    {
        /// <summary>
        /// ExcludeSameSiteUnspecifiedTreatedAsLax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ExcludeSameSiteUnspecifiedTreatedAsLax"))]
        ExcludeSameSiteUnspecifiedTreatedAsLax,
        /// <summary>
        /// ExcludeSameSiteNoneInsecure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ExcludeSameSiteNoneInsecure"))]
        ExcludeSameSiteNoneInsecure
    }
}