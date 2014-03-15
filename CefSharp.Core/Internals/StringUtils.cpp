// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

using namespace System::Diagnostics;

namespace CefSharp
{
    namespace Internals
    {
        [DebuggerStepThrough]
        String^ StringUtils::ToClr(const cef_string_t& cefStr)
        {
            return gcnew String(cefStr.str);
        }

        [DebuggerStepThrough]
        String^ StringUtils::ToClr(const CefString& cefStr)
        {
            return gcnew String(cefStr.c_str());
        }

        [DebuggerStepThrough]
        CefString StringUtils::ToNative(String^ str)
        {
            if (str == nullptr)
            {
                return CefString();
            }

            pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
            CefString cefStr(pStr);
            return cefStr;
        }

        [DebuggerStepThrough]
        void StringUtils::AssignNativeFromClr(cef_string_t& cefStr, String^ str)
        {
            cef_string_clear(&cefStr);

            if (str != nullptr)
            {
                pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
                cef_string_copy(pStr, str->Length, &cefStr);
            }
        }
    }
}
