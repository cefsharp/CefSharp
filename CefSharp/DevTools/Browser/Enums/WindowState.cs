// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// The state of the browser window.
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// normal
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("normal"))]
        Normal,
        /// <summary>
        /// minimized
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("minimized"))]
        Minimized,
        /// <summary>
        /// maximized
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("maximized"))]
        Maximized,
        /// <summary>
        /// fullscreen
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("fullscreen"))]
        Fullscreen
    }
}