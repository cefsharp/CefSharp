// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Enum of AudioNode::ChannelInterpretation from the spec
    /// </summary>
    public enum ChannelInterpretation
    {
        /// <summary>
        /// discrete
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("discrete"))]
        Discrete,
        /// <summary>
        /// speakers
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("speakers"))]
        Speakers
    }
}