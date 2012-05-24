#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IMenuHandler
    {
    public:
        bool OnBeforeMenu(IWebBrowser^ browser);
    };
}