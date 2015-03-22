// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private class JavascriptMethodHandler : public CefV8Handler
    {
    private:
        gcroot<Func<array<Object^>^, BrowserProcessResponse^>^> _method;
        gcroot<JavascriptCallbackRegistry^> _callbackRegistry;

    public:
        JavascriptMethodHandler(Func<array<Object^>^, BrowserProcessResponse^>^ method, JavascriptCallbackRegistry^ callbackRegistry);

        ~JavascriptMethodHandler();

        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);

        CefRefPtr<CefV8Value> ConvertToCefObject(Object^ obj);

        IMPLEMENT_REFCOUNTING(JavascriptMethodHandler)
    };
}