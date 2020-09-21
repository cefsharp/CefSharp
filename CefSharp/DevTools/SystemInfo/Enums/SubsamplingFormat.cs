// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// YUV subsampling type of the pixels of a given image.
    /// </summary>
    public enum SubsamplingFormat
    {
        /// <summary>
        /// yuv420
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("yuv420"))]
        Yuv420,
        /// <summary>
        /// yuv422
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("yuv422"))]
        Yuv422,
        /// <summary>
        /// yuv444
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("yuv444"))]
        Yuv444
    }
}