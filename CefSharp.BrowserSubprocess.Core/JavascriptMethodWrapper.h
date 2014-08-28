// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"
#include "include/cef_v8.h"

#include "JavascriptObjectWrapper.h"
#include "JavascriptMethodHandler.h"

using namespace CefSharp::Internals;
using namespace System::Runtime::Serialization;

namespace CefSharp
{
    public ref class JavascriptMethodWrapper
    {
    private:
        MCefRefPtr<JavascriptMethodHandler> _javascriptMethodHandler;
        JavascriptMethod^ _javascriptMethod;
        int _ownerId;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptMethodWrapper(JavascriptMethod^ javascriptMethod, int ownerId)
        {
            _javascriptMethod = javascriptMethod;
            _ownerId = ownerId;
            _javascriptMethodHandler = new JavascriptMethodHandler(gcnew Func<array<Object^>^, Object^>(this, &JavascriptMethodWrapper::Execute));
        }

        virtual void Bind()
        {
            auto methodName = StringUtils::ToNative(_javascriptMethod->JavascriptName);
            auto v8Value = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

            V8Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);
        };

        Object^ Execute(array<Object^>^ parameters);
    };
}