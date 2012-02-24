#include "stdafx.h"
#pragma once

namespace CefSharp
{
    CefRefPtr<CefV8Value> convertToCef(Object^ obj, Type^ type);
    Object^ convertFromCef(CefRefPtr<CefV8Value> obj);
}