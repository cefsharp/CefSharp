// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

namespace CefSharp
{
    private class JavascriptMethodHandler : public CefV8Handler
    {
    private:
        gcroot<Func<array<Object^>^, BrowserProcessResponse^>^> _method;
        gcroot<JavascriptCallbackRegistry^> _callbackRegistryReference;

    public:
        JavascriptMethodHandler(Func<array<Object^>^, BrowserProcessResponse^>^ method, JavascriptCallbackRegistry^ callbackRegistryReference)
        {
            _method = method;
            _callbackRegistryReference = callbackRegistryReference;
        }

        ~JavascriptMethodHandler()
        {
            delete _method;
            _callbackRegistryReference = nullptr;
            _method = nullptr;
        }

        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception) OVERRIDE;

        CefRefPtr<CefV8Value> ConvertToCefObject(Object^ obj);

        IMPLEMENT_REFCOUNTING(JavascriptMethodHandler)
    };
}