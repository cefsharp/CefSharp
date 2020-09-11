// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Stylesheet type: "injected" for stylesheets injected via extension, "user-agent" for user-agent
    /// stylesheets, "inspector" for stylesheets created by the inspector (i.e. those holding the "via
    /// inspector" rules), "regular" for regular stylesheets.
    /// </summary>
    public enum StyleSheetOrigin
    {
        /// <summary>
        /// injected
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("injected"))]
        Injected,
        /// <summary>
        /// user-agent
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("user-agent"))]
        UserAgent,
        /// <summary>
        /// inspector
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("inspector"))]
        Inspector,
        /// <summary>
        /// regular
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("regular"))]
        Regular
    }
}