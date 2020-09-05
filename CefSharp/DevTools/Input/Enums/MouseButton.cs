// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    /// <summary>
    /// MouseButton
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None,
        /// <summary>
        /// left
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("left"))]
        Left,
        /// <summary>
        /// middle
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("middle"))]
        Middle,
        /// <summary>
        /// right
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("right"))]
        Right,
        /// <summary>
        /// back
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("back"))]
        Back,
        /// <summary>
        /// forward
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("forward"))]
        Forward
    }
}