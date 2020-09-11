// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Enum of BaseAudioContext types
    /// </summary>
    public enum ContextType
    {
        /// <summary>
        /// realtime
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("realtime"))]
        Realtime,
        /// <summary>
        /// offline
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("offline"))]
        Offline
    }
}