// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "TypeUtils.h"
#include "JavascriptMethodHandler.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    JavascriptMethodHandler::JavascriptMethodHandler(Func<array<Object^>^, BrowserProcessResponse^>^ method, JavascriptCallbackRegistry^ callbackRegistry)
    {
        _method = method;
        _callbackRegistry = callbackRegistry;
    }

    JavascriptMethodHandler::~JavascriptMethodHandler()
    {
        delete _method;
        delete _callbackRegistry;
    }

    bool JavascriptMethodHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
    {
        auto parameter = gcnew array<Object^>(arguments.size());

        for (std::vector<CefRefPtr<CefV8Value>>::size_type i = 0; i != arguments.size(); i++)
        {
            if (arguments[i]->IsFunction())
            {
                parameter[i] = _callbackRegistry->Register(CefV8Context::GetCurrentContext(), arguments[i]);
            }
            else
            {
                parameter[i] = TypeUtils::ConvertFromCef(arguments[i]);
            }
        }

        try
        {
            auto response = _method->Invoke(parameter);

            retval = TypeUtils::ConvertToCef(response->Result, nullptr);
            if (!response->Success)
            {
                exception = StringUtils::ToNative(response->Message);
            }
        }
        catch (Exception^ ex)
        {
            exception = StringUtils::ToNative(ex->Message);
        }

        //NOTE: Return true otherwise exception is ignored
        return true;
    }
}