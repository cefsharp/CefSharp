#include "stdafx.h"

#pragma once

using namespace System;

namespace CefSharp
{
    String^ convertToString(cef_string_t& cefStr);
    String^ convertToString(CefString& cefStr);
    CefString convertFromString(String^ str);
    void assignFromString(cef_string_t& cefStrT, String^ str);
}