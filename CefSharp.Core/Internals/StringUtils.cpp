// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Diagnostics;
using namespace System::Collections::Generic;

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
        List<String^>^ StringUtils::ToClr(const std::vector<CefString>& cefStr)
        {
            auto result = gcnew List<String^>();

            for each(CefString s in cefStr)
            {
                result->Add(StringUtils::ToClr(s));
            }

            return result;
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
        std::vector<CefString> StringUtils::ToNative(List<String^>^ str)
        {
            if (str == nullptr)
            {
                return std::vector<CefString>();
            }

            std::vector<CefString> result = std::vector<CefString>();

            for each (String^ s in str)
            {
                result.push_back(StringUtils::ToNative(s));
            }

            return result;
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
