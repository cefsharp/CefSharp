// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        None = 0,
        Error = 1 << 0,
        Paused = 1 << 1,
        Muted = 1 << 2,
        Loop = 1 << 3,
        CanSave = 1 << 4,
        HasAudio = 1 << 5,
        HasVideo = 1 << 6,
        ControlRootElement = 1 << 7,
        CanPrint = 1 << 8,
        CanRotate = 1 << 9,
    }
}
