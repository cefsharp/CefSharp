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
        Other,
        Csp,
        MixedContent,
        Origin,
        Inspector,
        SubresourceFilter,
        ContentType,
        CollapsedByClient,
        CoepFrameResourceNeedsCoepHeader,
        CoopSandboxedIframeCannotNavigateToCoopPage,
        CorpNotSameOrigin,
        CorpNotSameOriginAfterDefaultedToSameOriginByCoep,
        CorpNotSameSite
    }
}