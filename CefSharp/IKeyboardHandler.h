// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public enum class KeyType
    {
        RawKeyDown = KEYEVENT_RAWKEYDOWN,
        KeyDown = KEYEVENT_KEYDOWN,
        KeyUp = KEYEVENT_KEYUP,
        Char = KEYEVENT_CHAR,
    };

    public interface class IKeyboardHandler
    {
    public:
        bool OnKeyEvent(IWebBrowser^ browser, KeyType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript);
    };
}