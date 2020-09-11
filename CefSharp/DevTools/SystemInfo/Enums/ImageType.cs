// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Image format of a given image.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// jpeg
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("jpeg"))]
        Jpeg,
        /// <summary>
        /// webp
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("webp"))]
        Webp,
        /// <summary>
        /// unknown
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("unknown"))]
        Unknown
    }
}