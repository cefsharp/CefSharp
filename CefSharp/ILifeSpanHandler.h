#include "Stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class ILifeSpanHandler
    {
    public:
        bool OnBeforePopup(IWebBrowser^ browser, String^ url, int% x, int% y, int% width, int% height);
        void OnBeforeClose(IWebBrowser^ browser);
    };
}