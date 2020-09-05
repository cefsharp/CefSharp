// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// PermissionSetting
    /// </summary>
    public enum PermissionSetting
    {
        /// <summary>
        /// granted
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("granted"))]
        Granted,
        /// <summary>
        /// denied
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("denied"))]
        Denied,
        /// <summary>
        /// prompt
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("prompt"))]
        Prompt
    }
}