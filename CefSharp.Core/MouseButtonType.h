// Copyright � 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_browser.h"

namespace CefSharp
{
    public enum class MouseButtonType
    {
        Left = CefBrowserHost::MouseButtonType::MBT_LEFT,
        Middle = CefBrowserHost::MouseButtonType::MBT_MIDDLE,
        Right = CefBrowserHost::MouseButtonType::MBT_RIGHT
    };

    /// <summary>
    /// Supported event bit flags.
    /// </summary>
    [FlagsAttribute]
    public enum class CefEventFlags : UInt32
    {
        None = 0,

        CapsLockOn = 1 << 0,

        ShiftDown = 1 << 1,
        ControlDown = 1 << 2,
        AltDown = 1 << 3,

        LeftMouseButton = 1 << 4,
        MiddleMouseButton = 1 << 5,
        RightMouseButton = 1 << 6,

        /// <summary>
        /// Mac OS-X command key.
        /// </summary>
        CommandDown = 1 << 7,

        NumLockOn = 1 << 8,
        IsKeyPad = 1 << 9,
        IsLeft = 1 << 10,
        IsRight = 1 << 11,
    };
}