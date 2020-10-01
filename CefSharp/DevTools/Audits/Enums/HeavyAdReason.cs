// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// HeavyAdReason
    /// </summary>
    public enum HeavyAdReason
    {
        /// <summary>
        /// NetworkTotalLimit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("NetworkTotalLimit"))]
        NetworkTotalLimit,
        /// <summary>
        /// CpuTotalLimit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CpuTotalLimit"))]
        CpuTotalLimit,
        /// <summary>
        /// CpuPeakLimit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("CpuPeakLimit"))]
        CpuPeakLimit
    }
}