// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// SafetyTipStatus
    /// </summary>
    public enum SafetyTipStatus
    {
        /// <summary>
        /// badReputation
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("badReputation"))]
        BadReputation,
        /// <summary>
        /// lookalike
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("lookalike"))]
        Lookalike
    }
}