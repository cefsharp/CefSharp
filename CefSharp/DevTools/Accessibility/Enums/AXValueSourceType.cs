// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// Enum of possible property sources.
    /// </summary>
    public enum AXValueSourceType
    {
        /// <summary>
        /// attribute
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("attribute"))]
        Attribute,
        /// <summary>
        /// implicit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("implicit"))]
        Implicit,
        /// <summary>
        /// style
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("style"))]
        Style,
        /// <summary>
        /// contents
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("contents"))]
        Contents,
        /// <summary>
        /// placeholder
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("placeholder"))]
        Placeholder,
        /// <summary>
        /// relatedElement
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("relatedElement"))]
        RelatedElement
    }
}