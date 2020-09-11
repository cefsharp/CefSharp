// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Enum of AudioContextState from the spec
    /// </summary>
    public enum ContextState
    {
        /// <summary>
        /// suspended
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("suspended"))]
        Suspended,
        /// <summary>
        /// running
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("running"))]
        Running,
        /// <summary>
        /// closed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("closed"))]
        Closed
    }
}