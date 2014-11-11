// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "TypeUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private class JavascriptMethodHandler : public CefV8Handler
    {
    private:
        gcroot<Func<array<Object^>^, BrowserProcessResponse^>^> _method;

    public:
        JavascriptMethodHandler(Func<array<Object^>^, BrowserProcessResponse^>^ method)
        {
            _method = method;
        }

        ~JavascriptMethodHandler()
        {
            delete _method;
        }

        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
        {
            auto parameter = gcnew array<Object^>(arguments.size());

            for (std::vector<CefRefPtr<CefV8Value>>::size_type i = 0; i != arguments.size(); i++)
            {
                parameter[i] = TypeUtils::ConvertFromCef(arguments[i]);
            }

            auto response = _method->Invoke(parameter);

            retval = TypeUtils::ConvertToCef(response->Result, nullptr);
            if(!response->Success)
            {
                exception = StringUtils::ToNative(response->Message);
            }
            //NOTE: Return true otherwise exception is ignored
            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptMethodHandler)
    };
}