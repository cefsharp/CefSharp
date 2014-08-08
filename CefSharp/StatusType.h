#include "Stdafx.h"
#pragma once

namespace CefSharp
{
    public enum class StatusType
    {
        Text = STATUSTYPE_TEXT,
        MouseoverUrl = STATUSTYPE_MOUSEOVER_URL,
        KeyboardFocusUrl = STATUSTYPE_KEYBOARD_FOCUS_URL,
    };
}