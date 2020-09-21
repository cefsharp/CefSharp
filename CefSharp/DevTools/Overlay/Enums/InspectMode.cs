// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// InspectMode
    /// </summary>
    public enum InspectMode
    {
        /// <summary>
        /// searchForNode
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("searchForNode"))]
        SearchForNode,
        /// <summary>
        /// searchForUAShadowDOM
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("searchForUAShadowDOM"))]
        SearchForUAShadowDOM,
        /// <summary>
        /// captureAreaScreenshot
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("captureAreaScreenshot"))]
        CaptureAreaScreenshot,
        /// <summary>
        /// showDistances
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("showDistances"))]
        ShowDistances,
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None
    }
}