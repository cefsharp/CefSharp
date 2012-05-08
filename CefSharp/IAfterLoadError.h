#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public interface class IAfterLoadError
    {
    public:
        bool HandleLoadError();
    };
}