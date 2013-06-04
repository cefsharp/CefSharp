#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class ILoadHandler
    {
    public:
        bool OnLoadError(IWebBrowser^ browser, String^ url, int errorCode, String^% errorText);
    };
}