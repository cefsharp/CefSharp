// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
}