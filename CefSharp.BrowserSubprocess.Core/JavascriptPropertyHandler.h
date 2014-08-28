// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "TypeUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private class JavascriptPropertyHandler : public CefV8Accessor
    {
		gcroot<Func<String^, Object^>^> _getter;
		gcroot<Action<String^, Object^>^> _setter;

    public:
		JavascriptPropertyHandler(Func<String^, Object^>^ getter, Action<String^, Object^>^ setter)
        {
            _getter = getter;
            _setter = setter;
        }

        virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
            CefString& exception) override
        {
            System::Diagnostics::Debugger::Break();
            auto propertyName = StringUtils::ToClr(name);
			auto result = _getter->Invoke(propertyName);
            retval = TypeUtils::ConvertToCef(result, nullptr);
            return true;
        }

        virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
            CefString& exception) override
        {
            System::Diagnostics::Debugger::Break();
            auto propertyName = StringUtils::ToClr(name);
            auto managedValue = TypeUtils::ConvertFromCef(value);
            _setter->Invoke(propertyName, managedValue);
            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptPropertyHandler)
    };
}
