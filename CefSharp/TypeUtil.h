#include "stdafx.h"
#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    CefRefPtr<CefV8Value> convertToCef(Object^ obj, Type^ type);
    Object^ convertFromCef(CefRefPtr<CefV8Value> obj);
}