// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// The referring-policy used for the navigation.
    /// </summary>
    public enum ReferrerPolicy
    {
        /// <summary>
        /// noReferrer
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("noReferrer"))]
        NoReferrer,
        /// <summary>
        /// noReferrerWhenDowngrade
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("noReferrerWhenDowngrade"))]
        NoReferrerWhenDowngrade,
        /// <summary>
        /// origin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("origin"))]
        Origin,
        /// <summary>
        /// originWhenCrossOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("originWhenCrossOrigin"))]
        OriginWhenCrossOrigin,
        /// <summary>
        /// sameOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("sameOrigin"))]
        SameOrigin,
        /// <summary>
        /// strictOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("strictOrigin"))]
        StrictOrigin,
        /// <summary>
        /// strictOriginWhenCrossOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("strictOriginWhenCrossOrigin"))]
        StrictOriginWhenCrossOrigin,
        /// <summary>
        /// unsafeUrl
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("unsafeUrl"))]
        UnsafeUrl
    }
}