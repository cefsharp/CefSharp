#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IBeforeMenu
    {
    public:
        // Return false to display the default context menu
        // or true to cancel the display.
        bool HandleBeforeMenu();
    };
}