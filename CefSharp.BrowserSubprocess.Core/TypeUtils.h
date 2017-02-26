// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

using namespace System;

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
        /// Converts a Chromium V8 value to a (managed) .NET object
        /// using a JavascriptCallbackRegistry param to convert any
        /// anonymous function to IJavascriptCallback, if callbackRegistry
        /// is nullptr will use nullptr to each anonymous function instead.
        /// </summary>
        /// <param name="obj">The V8 value that should be converted.</param>
        /// <param name="callbackRegistry">Instance of JavascriptCallbackRegistry to manage IJavascriptCallback instances.</param>
        /// <returns>A corresponding .NET object.</returns>
        static Object^ ConvertFromCef(CefRefPtr<CefV8Value> obj, JavascriptCallbackRegistry^ callbackRegistry);

        /// <summary>
        /// Converts a Chromium V8 CefTime (Date) to a (managed) .NET DateTime.
        /// </summary>
        /// <param name="obj">The CefTime value that should be converted.</param>
        /// <returns>A corresponding .NET DateTime.</returns>
        static DateTime ConvertCefTimeToDateTime(CefTime time);
                
        /// <summary>
        /// Converts a a (managed) .NET DateTime to Chromium V8 CefTime (Date).
        /// </summary>
        /// <param name="obj">The DateTime value that should be converted.</param>
        /// <returns>A corresponding CefTime (epoch).</returns>
        static CefTime ConvertDateTimeToCefTime(DateTime dateTime);
    };
}