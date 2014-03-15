// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

namespace CefSharp
{
    private class TypeUtils
    {
    public:
        /// <summary>
        /// Converts a .NET object to an (unmanaged) Chromium V8 object.
        /// </summary>
        /// <param name="obj">The .NET object that should be converted.</param>
        /// <param name="type">The type of the source object. If this parameter is a null reference, the type will be determined
        /// automatically.</param>
        /// <returns>A corresponding V8 value.</returns>
        static CefRefPtr<CefV8Value> ConvertToCef(Object^ obj, Type^ type);

        /// <summary>
        /// Converts a Chromium V8 value to a (managed) .NET object.
        /// </summary>
        /// <param name="obj">The V8 value that should be converted.</param>
        /// <returns>A corresponding .NET object.</returns>
        static Object^ ConvertFromCef(CefRefPtr<CefV8Value> obj);
    };
}