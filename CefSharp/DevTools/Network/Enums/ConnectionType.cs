// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// The underlying connection technology that the browser is supposedly using.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None,
        /// <summary>
        /// cellular2g
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cellular2g"))]
        Cellular2g,
        /// <summary>
        /// cellular3g
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cellular3g"))]
        Cellular3g,
        /// <summary>
        /// cellular4g
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cellular4g"))]
        Cellular4g,
        /// <summary>
        /// bluetooth
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("bluetooth"))]
        Bluetooth,
        /// <summary>
        /// ethernet
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("ethernet"))]
        Ethernet,
        /// <summary>
        /// wifi
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("wifi"))]
        Wifi,
        /// <summary>
        /// wimax
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("wimax"))]
        Wimax,
        /// <summary>
        /// other
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("other"))]
        Other
    }
}