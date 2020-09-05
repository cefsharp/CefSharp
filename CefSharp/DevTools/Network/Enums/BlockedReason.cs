// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// The reason why request was blocked.
    /// </summary>
    public enum BlockedReason
    {
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other,
        /// <summary>
        /// csp
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("csp"))]
        Csp,
        /// <summary>
        /// mixed-content
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("mixed-content"))]
        MixedContent,
        /// <summary>
        /// origin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("origin"))]
        Origin,
        /// <summary>
        /// inspector
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("inspector"))]
        Inspector,
        /// <summary>
        /// subresource-filter
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("subresource-filter"))]
        SubresourceFilter,
        /// <summary>
        /// content-type
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("content-type"))]
        ContentType,
        /// <summary>
        /// collapsed-by-client
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("collapsed-by-client"))]
        CollapsedByClient,
        /// <summary>
        /// coep-frame-resource-needs-coep-header
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("coep-frame-resource-needs-coep-header"))]
        CoepFrameResourceNeedsCoepHeader,
        /// <summary>
        /// coop-sandboxed-iframe-cannot-navigate-to-coop-page
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("coop-sandboxed-iframe-cannot-navigate-to-coop-page"))]
        CoopSandboxedIframeCannotNavigateToCoopPage,
        /// <summary>
        /// corp-not-same-origin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("corp-not-same-origin"))]
        CorpNotSameOrigin,
        /// <summary>
        /// corp-not-same-origin-after-defaulted-to-same-origin-by-coep
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("corp-not-same-origin-after-defaulted-to-same-origin-by-coep"))]
        CorpNotSameOriginAfterDefaultedToSameOriginByCoep,
        /// <summary>
        /// corp-not-same-site
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("corp-not-same-site"))]
        CorpNotSameSite
    }
}