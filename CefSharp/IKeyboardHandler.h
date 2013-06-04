#include "stdafx.h"
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