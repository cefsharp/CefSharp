// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// ClientNavigationDisposition
    /// </summary>
    public enum ClientNavigationDisposition
    {
        /// <summary>
        /// currentTab
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("currentTab"))]
        CurrentTab,
        /// <summary>
        /// newTab
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("newTab"))]
        NewTab,
        /// <summary>
        /// newWindow
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("newWindow"))]
        NewWindow,
        /// <summary>
        /// download
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("download"))]
        Download
    }
}