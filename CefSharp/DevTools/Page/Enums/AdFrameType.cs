// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether a frame has been identified as an ad.
    /// </summary>
    public enum AdFrameType
    {
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None,
        /// <summary>
        /// child
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("child"))]
        Child,
        /// <summary>
        /// root
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("root"))]
        Root
    }
}