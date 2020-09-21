// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// ClientNavigationReason
    /// </summary>
    public enum ClientNavigationReason
    {
        /// <summary>
        /// formSubmissionGet
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("formSubmissionGet"))]
        FormSubmissionGet,
        /// <summary>
        /// formSubmissionPost
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("formSubmissionPost"))]
        FormSubmissionPost,
        /// <summary>
        /// httpHeaderRefresh
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("httpHeaderRefresh"))]
        HttpHeaderRefresh,
        /// <summary>
        /// scriptInitiated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("scriptInitiated"))]
        ScriptInitiated,
        /// <summary>
        /// metaTagRefresh
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("metaTagRefresh"))]
        MetaTagRefresh,
        /// <summary>
        /// pageBlockInterstitial
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("pageBlockInterstitial"))]
        PageBlockInterstitial,
        /// <summary>
        /// reload
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("reload"))]
        Reload,
        /// <summary>
        /// anchorClick
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("anchorClick"))]
        AnchorClick
    }
}