#include "stdafx.h"

namespace CefSharp
{
    String^ convertToString(cef_string_t& cefStr)
    {
        return gcnew String(cefStr.str);
    }
    
    String^ convertToString(CefString& cefStr)
    {
        return gcnew String(cefStr.c_str());
    }

    CefString convertFromString(String^ str)
    {
        pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
        CefString cefStr(pStr);
        return cefStr;
    }

    void assignFromString(cef_string_t& cefStrT, String^ str)
    {
        cef_string_clear(&cefStrT);
        if(str != nullptr)
        {
            pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
            cef_string_copy(pStr, str->Length, &cefStrT);
        }
    }

}