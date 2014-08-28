// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptPropertyHandler.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptPropertyWrapper
    {
    private:
        JavascriptProperty^ _javascriptProperty;
        int _ownerId;

    internal:
        MCefRefPtr<CefV8Value> V8Value;
        MCefRefPtr<JavascriptPropertyHandler> JsPropertyHandler;

    public:
        JavascriptPropertyWrapper(JavascriptProperty^ javascriptProperty, int ownerId)
        {
            _javascriptProperty = javascriptProperty;
            _ownerId = ownerId;
            JsPropertyHandler = new JavascriptPropertyHandler(
                gcnew Func<Object^>(this, &JavascriptPropertyWrapper::GetProperty),
                gcnew Action<Object^>(this, &JavascriptPropertyWrapper::SetProperty)
            );
        }

        virtual void Bind()
        {
            auto methodName = StringUtils::ToNative(_javascriptProperty->JavascriptName);
            auto clrMethodName = _javascriptProperty->JavascriptName;
            
            if(_javascriptProperty->IsComplexType)
            {
                //TODO: Implement complex type
                //TODO: Should have property of type JavascriptObjectWrapper here
                //auto v8Value = V8Value->CreateObject(_javascriptPropertyHandler.get());

                //JsObject->V8Value = v8Value;

                //JsObject->Bind();

                //_owner->V8Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);
            }
            else
            {
                V8Value->SetValue(methodName, V8_ACCESS_CONTROL_DEFAULT, V8_PROPERTY_ATTRIBUTE_NONE);
            }
        };

        void SetProperty(Object^ value);
        Object^ GetProperty();
    };
}