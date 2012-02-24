#include "stdafx.h"

#include "StringUtil.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    String^ StringUtil::ToClr(IntPtr ptr)
    {
        return Marshal::PtrToStringAnsi(ptr);
    }

    /*
    String^ StringUtil::ToClr(const cef_string_t& cefStr)
    {
        return gcnew String(cefStr.str);
    }

    String^ StringUtil::ToClr(const CefString& cefStr)
    {
        return gcnew String(cefStr.c_str());
    }
    */

    CefString StringUtil::ToNative(String^ str)
    {
        pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
        CefString cefStr(pStr);
        return cefStr;
    }
}