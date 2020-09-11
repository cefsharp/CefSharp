// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// SameSiteCookieWarningReason
    /// </summary>
    public enum SameSiteCookieWarningReason
    {
        /// <summary>
        /// WarnSameSiteUnspecifiedCrossSiteContext
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteUnspecifiedCrossSiteContext"))]
        WarnSameSiteUnspecifiedCrossSiteContext,
        /// <summary>
        /// WarnSameSiteNoneInsecure
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteNoneInsecure"))]
        WarnSameSiteNoneInsecure,
        /// <summary>
        /// WarnSameSiteUnspecifiedLaxAllowUnsafe
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteUnspecifiedLaxAllowUnsafe"))]
        WarnSameSiteUnspecifiedLaxAllowUnsafe,
        /// <summary>
        /// WarnSameSiteStrictLaxDowngradeStrict
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteStrictLaxDowngradeStrict"))]
        WarnSameSiteStrictLaxDowngradeStrict,
        /// <summary>
        /// WarnSameSiteStrictCrossDowngradeStrict
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteStrictCrossDowngradeStrict"))]
        WarnSameSiteStrictCrossDowngradeStrict,
        /// <summary>
        /// WarnSameSiteStrictCrossDowngradeLax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteStrictCrossDowngradeLax"))]
        WarnSameSiteStrictCrossDowngradeLax,
        /// <summary>
        /// WarnSameSiteLaxCrossDowngradeStrict
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteLaxCrossDowngradeStrict"))]
        WarnSameSiteLaxCrossDowngradeStrict,
        /// <summary>
        /// WarnSameSiteLaxCrossDowngradeLax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("WarnSameSiteLaxCrossDowngradeLax"))]
        WarnSameSiteLaxCrossDowngradeLax
    }
}