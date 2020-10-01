// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Enum of AudioNode::ChannelCountMode from the spec
    /// </summary>
    public enum ChannelCountMode
    {
        /// <summary>
        /// clamped-max
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("clamped-max"))]
        ClampedMax,
        /// <summary>
        /// explicit
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("explicit"))]
        Explicit,
        /// <summary>
        /// max
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("max"))]
        Max
    }
}