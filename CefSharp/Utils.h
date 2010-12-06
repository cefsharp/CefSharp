#include "stdafx.h"

#pragma once

using namespace System;

namespace CefSharp
{
    String^ convertToString(const cef_string_t& cefStr);
    String^ convertToString(const CefString& cefStr);
    CefString convertFromString(String^ str);
    void assignFromString(cef_string_t& cefStrT, String^ str);
}