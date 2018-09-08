// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "TypeUtils.h"

namespace CefSharp
{
    private class JavascriptPropertyHandler : public CefV8Accessor
    {
        gcroot<Func<String^, BrowserProcessResponse^>^> _getter;
        gcroot<Func<String^, Object^, BrowserProcessResponse^>^> _setter;

    public:
        JavascriptPropertyHandler(Func<String^, BrowserProcessResponse^>^ getter, Func<String^, Object^, BrowserProcessResponse^>^ setter)
        {
            _getter = getter;
            _setter = setter;
        }

        ~JavascriptPropertyHandler()
        {
            delete _getter;
            delete _setter;
        }


        virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
            CefString& exception) override
        {
            //System::Diagnostics::Debugger::Break();
            auto propertyName = StringUtils::ToClr(name);
            auto response = _getter->Invoke(propertyName);
            retval = TypeUtils::ConvertToCef(response->Result, nullptr);
            if (!response->Success)
            {
                exception = StringUtils::ToNative(response->Message);
            }
            //NOTE: Return true otherwise exception is ignored
            return true;
        }

        virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
            CefString& exception) override
        {
            //System::Diagnostics::Debugger::Break();
            auto propertyName = StringUtils::ToClr(name);
            auto managedValue = TypeUtils::ConvertFromCef(value, nullptr);
            auto response = _setter->Invoke(propertyName, managedValue);
            if (!response->Success)
            {
                exception = StringUtils::ToNative(response->Message);
            }
            //NOTE: Return true otherwise exception is ignored
            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptPropertyHandler)
    };
}
