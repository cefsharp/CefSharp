// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// Enum indicating the reason a response has been blocked. These reasons are
    /// refinements of the net error BLOCKED_BY_RESPONSE.
    /// </summary>
    public enum BlockedByResponseReason
    {
        /// <summary>
        /// CoepFrameResourceNeedsCoepHeader
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CoepFrameResourceNeedsCoepHeader"))]
        CoepFrameResourceNeedsCoepHeader,
        /// <summary>
        /// CoopSandboxedIFrameCannotNavigateToCoopPage
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CoopSandboxedIFrameCannotNavigateToCoopPage"))]
        CoopSandboxedIFrameCannotNavigateToCoopPage,
        /// <summary>
        /// CorpNotSameOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CorpNotSameOrigin"))]
        CorpNotSameOrigin,
        /// <summary>
        /// CorpNotSameOriginAfterDefaultedToSameOriginByCoep
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CorpNotSameOriginAfterDefaultedToSameOriginByCoep"))]
        CorpNotSameOriginAfterDefaultedToSameOriginByCoep,
        /// <summary>
        /// CorpNotSameSite
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CorpNotSameSite"))]
        CorpNotSameSite
    }
}