#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IAfterResponse
    {
    public:
        void HandleSetCookie(String^ cookie);
    };
}