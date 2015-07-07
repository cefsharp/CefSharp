// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include<include\cef_v8.h>

using namespace CefSharp::Internals;

namespace CefSharp
{
    namespace Internals
    {
        private ref class JavascriptCallbackWrapper
        {
        private:
            MCefRefPtr<CefV8Value> value;
            MCefRefPtr<CefV8Context> context;

        internal:
            CefRefPtr<CefV8Value> GetValue();
            CefRefPtr<CefV8Context> GetContext();

        public:
            JavascriptCallbackWrapper(CefRefPtr<CefV8Value> value, CefRefPtr<CefV8Context> context)
                : value(value), context(context) 
            {
            }

            !JavascriptCallbackWrapper();
            ~JavascriptCallbackWrapper();

            JavascriptResponse^ Execute(array<Object^>^ parms);
        };
    }
}