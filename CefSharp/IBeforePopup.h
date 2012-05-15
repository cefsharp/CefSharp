#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IBeforePopup
    {
    public:
        // Return false to have the framework create the new popup window.
        // Return true to cancel creation of the popup window.
        bool HandleBeforePopup(IWebBrowser^ browser, String^ url, int% x, int% y, int% width, int% height);
    };
}