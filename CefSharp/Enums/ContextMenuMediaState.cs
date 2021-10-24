// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Supported context menu media state bit flags.
    /// </summary>
    [Flags]
    public enum ContextMenuMediaState
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Error
        /// </summary>
        Error = 1 << 0,
        /// <summary>
        /// Paused
        /// </summary>
        Paused = 1 << 1,
        /// <summary>
        /// Muted
        /// </summary>
        Muted = 1 << 2,
        /// <summary>
        /// Loop
        /// </summary>
        Loop = 1 << 3,
        /// <summary>
        /// CanSave
        /// </summary>
        CanSave = 1 << 4,
        /// <summary>
        /// HasAudio
        /// </summary>
        HasAudio = 1 << 5,
        /// <summary>
        /// HasVideo
        /// </summary>
        HasVideo = 1 << 6,
        /// <summary>
        /// ControlRootElement
        /// </summary>
        ControlRootElement = 1 << 7,
        /// <summary>
        /// CanPrint
        /// </summary>
        CanPrint = 1 << 8,
        /// <summary>
        /// CanRotate
        /// </summary>
        CanRotate = 1 << 9,
    }
}
