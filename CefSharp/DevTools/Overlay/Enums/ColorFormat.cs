// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// ColorFormat
    /// </summary>
    public enum ColorFormat
    {
        /// <summary>
        /// rgb
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("rgb"))]
        Rgb,
        /// <summary>
        /// hsl
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("hsl"))]
        Hsl,
        /// <summary>
        /// hex
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("hex"))]
        Hex
    }
}