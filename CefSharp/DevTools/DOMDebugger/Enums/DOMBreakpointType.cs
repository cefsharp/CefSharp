// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// DOM breakpoint type.
    /// </summary>
    public enum DOMBreakpointType
    {
        /// <summary>
        /// subtree-modified
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("subtree-modified"))]
        SubtreeModified,
        /// <summary>
        /// attribute-modified
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("attribute-modified"))]
        AttributeModified,
        /// <summary>
        /// node-removed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("node-removed"))]
        NodeRemoved
    }
}