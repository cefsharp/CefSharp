// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"

namespace CefSharp
{
    public enum class PaintElementType
    {
        View = CefBrowserHost::PaintElementType::PET_VIEW,
        Popup = CefBrowserHost::PaintElementType::PET_POPUP
    };
}