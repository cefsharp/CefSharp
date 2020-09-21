// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Loading priority of a resource request.
    /// </summary>
    public enum ResourcePriority
    {
        /// <summary>
        /// VeryLow
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("VeryLow"))]
        VeryLow,
        /// <summary>
        /// Low
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Low"))]
        Low,
        /// <summary>
        /// Medium
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Medium"))]
        Medium,
        /// <summary>
        /// High
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("High"))]
        High,
        /// <summary>
        /// VeryHigh
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("VeryHigh"))]
        VeryHigh
    }
}