// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "TypeUtils.h"

namespace CefSharp
{
    private class JavascriptMethodHandler : public CefV8Handler
    {
    private:
        gcroot<Func<array<Object^>^, Object^>^> _method;

    public:
        JavascriptMethodHandler(Func<array<Object^>^, Object^>^ method)
        {
            _method = method;
        }

        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
        {
            auto parameter = gcnew array<Object^>(arguments.size());

            for (std::vector<CefRefPtr<CefV8Value>>::size_type i = 0; i != arguments.size(); i++)
            {
                parameter[i] = TypeUtils::ConvertFromCef(arguments[i]);
            }

            auto result = _method->Invoke(parameter);

            retval = TypeUtils::ConvertToCef(result, nullptr);
            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptMethodHandler)
    };
}