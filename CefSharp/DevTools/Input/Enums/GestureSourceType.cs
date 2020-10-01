// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    /// <summary>
    /// GestureSourceType
    /// </summary>
    public enum GestureSourceType
    {
        /// <summary>
        /// default
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("default"))]
        Default,
        /// <summary>
        /// touch
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("touch"))]
        Touch,
        /// <summary>
        /// mouse
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("mouse"))]
        Mouse
    }
}