#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public ref class StringUtil sealed
    {
    public:
        static String^ ToClr(IntPtr cefStr);
        static String^ ToClr(const cef_string_t& cefStr);
        static String^ ToClr(const CefString& cefStr);
        static CefString ToNative(String^ str);
    };
}