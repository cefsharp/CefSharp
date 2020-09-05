// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// CrossOriginOpenerPolicyValue
    /// </summary>
    public enum CrossOriginOpenerPolicyValue
    {
        /// <summary>
        /// SameOrigin
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameOrigin"))]
        SameOrigin,
        /// <summary>
        /// SameOriginAllowPopups
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameOriginAllowPopups"))]
        SameOriginAllowPopups,
        /// <summary>
        /// UnsafeNone
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("UnsafeNone"))]
        UnsafeNone,
        /// <summary>
        /// SameOriginPlusCoep
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("SameOriginPlusCoep"))]
        SameOriginPlusCoep
    }
}