// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"
#include "JavascriptObjectWrapper.h"
#include "JavascriptMethodHandler.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    [System::Runtime::Serialization::DataContractAttribute]
    private ref class JavascriptMethodWrapper : public JavascriptMethod
    {
    private:
        MCefRefPtr<JavascriptMethodHandler> _javascriptMethodHandler;
        JavascriptObjectWrapper^ _owner; 

    public:

        JavascriptMethodWrapper()
        {
            _javascriptMethodHandler = new JavascriptMethodHandler(gcnew Func<array<Object^>^, Object^>(this, &JavascriptMethodWrapper::Execute));
        }

        virtual void Bind(JavascriptObject^ owner) override
        {
            _owner = static_cast<JavascriptObjectWrapper^>(owner);
            
            auto methodName = StringUtils::ToNative(Description->JavascriptName);
            auto v8Value = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

            _owner->Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);
        };

        Object^ Execute(array<Object^>^ parameters );
    };
}