// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <vector>
#include <sstream>
#include "vcclr_local.h"
#include "include\cef_v8.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;

namespace CefSharp
{
    namespace Internals
    {
        private class StringUtils
        {
        public:
            /// <summary>
            /// Converts an unmanaged string to a (managed) .NET string.
            /// </summary>
            /// <param name="cefStr">The string that should be converted.</param>
            /// <returns>A .NET string.</returns>
            [DebuggerStepThrough]
            static String^ StringUtils::ToClr(const cef_string_t& cefStr)
            {
                return gcnew String(cefStr.str);
            }

            /// <summary>
            /// Converts an unmanaged string to a (managed) .NET string.
            /// </summary>
            /// <param name="cefStr">The string that should be converted.</param>
            /// <returns>A .NET string.</returns>
            [DebuggerStepThrough]
            static String^ StringUtils::ToClr(const CefString& cefStr)
            {
                return gcnew String(cefStr.c_str());
            }

            /// <summary>
            /// Converts an unmanaged vector of strings to a (managed) .NET List of strings.
            /// </summary>
            /// <param name="cefStr">The vector of strings that should be converted.</param>
            /// <returns>A .NET List of strings.</returns>
            [DebuggerStepThrough]
            static List<String^>^ ToClr(const std::vector<CefString>& cefStr)
            {
                auto result = gcnew List<String^>();

                for each(CefString s in cefStr)
                {
                    result->Add(StringUtils::ToClr(s));
                }

                return result;
            }

            /// <summary>
            /// Converts a .NET string to native (unmanaged) format. Note that this method does not allocate a new copy of the
            // string, but rather returns a pointer to the memory in the existing managed String object.
            /// </summary>
            /// <param name="str">The string that should be converted.</param>
            /// <returns>An unmanaged representation of the provided string, or an empty string if the input string is a nullptr.</returns>
            [DebuggerStepThrough]
            static CefString ToNative(String^ str)
            {
                if (str == nullptr)
                {
                    return CefString();
                }

                pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
                CefString cefStr(pStr);
                return cefStr;
            }

            /// <summary>
            /// Converts a .NET List of strings to native (unmanaged) format.
            /// </summary>
            /// <param name="str">The List of strings that should be converted.</param>
            /// <returns>An unmanaged representation of the provided List of strings, or an empty List if the input is a nullptr.</returns>
            [DebuggerStepThrough]
            static std::vector<CefString> ToNative(IEnumerable<String^>^ str)
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

            /// <summary>
            /// Assigns the provided cef_string_t object from the given .NET string.
            /// </summary>
            /// <param name="cefStr">The cef_string_t that should be updated.</param>
            /// <param name="str">The .NET string whose value should be used to update cefStr.</param>
            [DebuggerStepThrough]
            static void StringUtils::AssignNativeFromClr(cef_string_t& cefStr, String^ str)
            {
                cef_string_clear(&cefStr);

                if (str != nullptr)
                {
                    pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
                    cef_string_copy(pStr, str->Length, &cefStr);
                }
            }

            /// <summary>
            /// Creates a detailed expection string from a provided Cef V8 exception.
            /// </summary>
            /// <param name="exception">The exception which will be used as base for the message</param>
            [DebuggerStepThrough]
            static CefString CreateExceptionString(CefRefPtr<CefV8Exception> exception)
            {
                if (exception.get())
                {
                    std::wstringstream logMessageBuilder;
                    logMessageBuilder << exception->GetMessage().c_str() << L"\n@ ";
                    if (!exception->GetScriptResourceName().empty())
                    {
                        logMessageBuilder << exception->GetScriptResourceName().c_str();
                    }
                    logMessageBuilder << L":" << exception->GetLineNumber() << L":" << exception->GetStartColumn();
                    return CefString(logMessageBuilder.str());
                }
                
                return "Exception occured but the Cef V8 exception is null";
            }
        };
    }
}