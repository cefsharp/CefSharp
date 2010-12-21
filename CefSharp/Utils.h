#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    String^ toClr(const cef_string_t& cefStr);
    String^ toClr(const CefString& cefStr);
    CefString toNative(String^ str);
    void assignFromString(cef_string_t& cefStrT, String^ str);
}