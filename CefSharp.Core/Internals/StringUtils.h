﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        private class StringUtils
        {
        private:
            /// <summary>
            /// The default log count for inner exceptions.
            /// </summary>
            static const int INNER_EXCEPTION_LOG_COUNT = 5;
        public:
            /// <summary>
            /// Converts an unmanaged string to a (managed) .NET string.
            /// </summary>
            /// <param name="cefStr">The string that should be converted.</param>
            /// <returns>A .NET string.</returns>
            static String^ ToClr(const cef_string_t& cefStr);

            /// <summary>
            /// Converts an unmanaged string to a (managed) .NET string.
            /// </summary>
            /// <param name="cefStr">The string that should be converted.</param>
            /// <returns>A .NET string.</returns>
            static String^ ToClr(const CefString& cefStr);

            /// <summary>
            /// Converts an unmanaged vector of strings to a (managed) .NET List of strings.
            /// </summary>
            /// <param name="cefStr">The vector of strings that should be converted.</param>
            /// <returns>A .NET List of strings.</returns>
            static List<String^>^ ToClr(const std::vector<CefString>& cefStr);

            /// <summary>
            /// Converts a .NET string to native (unmanaged) format. Note that this method does not allocate a new copy of the
            // string, but rather returns a pointer to the memory in the existing managed String object.
            /// </summary>
            /// <param name="str">The string that should be converted.</param>
            /// <returns>An unmanaged representation of the provided string, or an empty string if the input string is a nullptr.</returns>
            static CefString ToNative(String^ str);

            /// <summary>
            /// Converts a .NET List of strings to native (unmanaged) format.
            /// </summary>
            /// <param name="str">The List of strings that should be converted.</param>
            /// <returns>An unmanaged representation of the provided List of strings, or an empty List if the input is a nullptr.</returns>
            static std::vector<CefString> ToNative(IEnumerable<String^>^ str);

            /// <summary>
            /// Assigns the provided cef_string_t object from the given .NET string.
            /// </summary>
            /// <param name="cefStr">The cef_string_t that should be updated.</param>
            /// <param name="str">The .NET string whose value should be used to update cefStr.</param>
            static void AssignNativeFromClr(cef_string_t& cefStr, String^ str);

            /// <summary>
            /// Creates a detailed expection string from a provided exception.
            /// </summary>
            /// <param name="ex">The exception which will be used as base for the message</param>
            /// <param name="limit">The optional limit for logging inner exceptions.</param>
            static String^ CreateExceptionString(Exception^ ex, int limit = INNER_EXCEPTION_LOG_COUNT);
        };
    }
}