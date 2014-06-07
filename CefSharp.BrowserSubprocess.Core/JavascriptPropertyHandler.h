// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "TypeUtils.h"

#pragma once

namespace CefSharp
{
    private class JavascriptPropertyHandler : public CefV8Accessor
    {
        gcroot<Func<Object^>^> _getter;
        gcroot<Action<Object^>^> _setter;

    public:
        JavascriptPropertyHandler(Func<Object^>^ getter, Action<Object^>^ setter)
        {
            _getter = getter;
            _setter = setter;
        }

        virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
            CefString& exception) override
        {
            auto result = _getter->Invoke();
            retval = TypeUtils::ConvertToCef(result, nullptr);
            return true;
        }

        virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
            CefString& exception) override
        {
            System::Diagnostics::Debugger::Break();
            auto managedValue = TypeUtils::ConvertFromCef(value);
            _setter->Invoke(managedValue);
            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptPropertyHandler)
    };
}
