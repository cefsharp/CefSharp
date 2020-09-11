// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// A unique identifier for the type of issue. Each type may use one of the
    public enum InspectorIssueCode
    {
        /// <summary>
        /// SameSiteCookieIssue
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameSiteCookieIssue"))]
        SameSiteCookieIssue,
        /// <summary>
        /// MixedContentIssue
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("MixedContentIssue"))]
        MixedContentIssue
    }
}