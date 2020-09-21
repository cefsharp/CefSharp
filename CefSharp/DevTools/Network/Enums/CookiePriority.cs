// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Represents the cookie's 'Priority' status:
    /// https://tools.ietf.org/html/draft-west-cookie-priority-00
    /// </summary>
    public enum CookiePriority
    {
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
        High
    }
}