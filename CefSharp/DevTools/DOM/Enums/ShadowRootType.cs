// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Shadow root type.
    /// </summary>
    public enum ShadowRootType
    {
        /// <summary>
        /// user-agent
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("user-agent"))]
        UserAgent,
        /// <summary>
        /// open
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("open"))]
        Open,
        /// <summary>
        /// closed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("closed"))]
        Closed
    }
}