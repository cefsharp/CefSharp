// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Javascript dialog type.
    /// </summary>
    public enum DialogType
    {
        /// <summary>
        /// alert
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("alert"))]
        Alert,
        /// <summary>
        /// confirm
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("confirm"))]
        Confirm,
        /// <summary>
        /// prompt
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("prompt"))]
        Prompt,
        /// <summary>
        /// beforeunload
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("beforeunload"))]
        Beforeunload
    }
}