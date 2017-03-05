// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"

namespace CefSharp
{
    void JavascriptObjectWrapper::Bind(JavascriptObject^ object, const CefRefPtr<CefV8Value>& v8Value, JavascriptCallbackRegistry^ callbackRegistry)
    {
        _objectId = object->Id;

        //Create property handler for get and set of Properties of this object
        _jsPropertyHandler = new JavascriptPropertyHandler(
            gcnew Func<String^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::GetProperty),
            gcnew Func<String^, Object^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::SetProperty)
            );

        //V8Value that represents this javascript object - only one per complex type
        auto javascriptObject = CefV8Value::CreateObject(_jsPropertyHandler.get(), NULL);
        auto objectName = StringUtils::ToNative(object->JavascriptName);
        v8Value->SetValue(objectName, javascriptObject, V8_PROPERTY_ATTRIBUTE_NONE);

        for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(object->Methods))
        {
            auto wrappedMethod = gcnew JavascriptMethodWrapper(object->Id, _browserProcess, callbackRegistry);
            wrappedMethod->Bind(method, javascriptObject);

            _wrappedMethods->Add(wrappedMethod);
        }

        for each (JavascriptProperty^ prop in Enumerable::OfType<JavascriptProperty^>(object->Properties))
        {
            auto wrappedproperty = gcnew JavascriptPropertyWrapper(object->Id, _browserProcess);
            wrappedproperty->Bind(prop, javascriptObject, callbackRegistry);

            _wrappedProperties->Add(wrappedproperty);
        }
    }

    BrowserProcessResponse^ JavascriptObjectWrapper::GetProperty(String^ memberName)
    {
        return _browserProcess->GetProperty(_objectId, memberName);
    };

    BrowserProcessResponse^ JavascriptObjectWrapper::SetProperty(String^ memberName, Object^ value)
    {
        return _browserProcess->SetProperty(_objectId, memberName, value);
    };
}