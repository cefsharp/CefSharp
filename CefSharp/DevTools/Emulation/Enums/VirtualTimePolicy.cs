// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// advance: If the scheduler runs out of immediate work, the virtual time base may fast forward to
    public enum VirtualTimePolicy
    {
        /// <summary>
        /// advance
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("advance"))]
        Advance,
        /// <summary>
        /// pause
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("pause"))]
        Pause,
        /// <summary>
        /// pauseIfNetworkFetchesPending
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("pauseIfNetworkFetchesPending"))]
        PauseIfNetworkFetchesPending
    }
}