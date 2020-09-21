// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// Enum of possible native property sources (as a subtype of a particular AXValueSourceType).
    /// </summary>
    public enum AXValueNativeSourceType
    {
        /// <summary>
        /// figcaption
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("figcaption"))]
        Figcaption,
        /// <summary>
        /// label
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("label"))]
        Label,
        /// <summary>
        /// labelfor
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("labelfor"))]
        Labelfor,
        /// <summary>
        /// labelwrapped
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("labelwrapped"))]
        Labelwrapped,
        /// <summary>
        /// legend
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("legend"))]
        Legend,
        /// <summary>
        /// tablecaption
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("tablecaption"))]
        Tablecaption,
        /// <summary>
        /// title
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("title"))]
        Title,
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other
    }
}