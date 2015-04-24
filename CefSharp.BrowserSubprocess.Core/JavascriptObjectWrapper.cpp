// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"

using namespace System;
using namespace CefSharp::Internals;

namespace CefSharp
{
    void JavascriptObjectWrapper::Bind()
	{
        auto objectName = StringUtils::ToNative(_object->JavascriptName);

        if (_object->IsNull) {
            auto javascriptObject = V8Value.get() ? V8Value->CreateNull() : CefV8Value::CreateNull();        
            if (V8Value.get() && !V8Value->HasValue(objectName))
                V8Value->SetValue(objectName, javascriptObject, V8_PROPERTY_ATTRIBUTE_NONE);
            else
                V8Value = javascriptObject;
            return;
        }

		//Create property handler for get and set of Properties of this object
		_jsPropertyHandler = new JavascriptPropertyHandler(
			gcnew Func<String^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::GetProperty),
			gcnew Func<String^, Object^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::SetProperty)
			);

		//V8Value that represents this javascript object - only one per complex type
        auto javascriptObject = V8Value.get() ? V8Value->CreateObject(_jsPropertyHandler.get()) : CefV8Value::CreateObject(_jsPropertyHandler.get());
        if (V8Value.get() && !V8Value->HasValue(objectName))
            V8Value->SetValue(objectName, javascriptObject, V8_PROPERTY_ATTRIBUTE_NONE);
        else
            V8Value = javascriptObject;
		for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(_object->Methods))
		{
			auto wrappedMethod = gcnew JavascriptMethodWrapper(method, _object->Id, _browserProcess, CallbackRegistry);
			wrappedMethod->V8Value = javascriptObject;
			wrappedMethod->Bind();

			_wrappedMethods->Add(wrappedMethod);
		}

		for each (JavascriptProperty^ prop in Enumerable::OfType<JavascriptProperty^>(_object->Properties))
		{
			auto wrappedproperty = gcnew JavascriptPropertyWrapper(prop, _object->Id, _browserProcess);
			wrappedproperty->V8Value = javascriptObject;
			wrappedproperty->Bind();

            _wrappedProperties->Add(prop->JavascriptName, wrappedproperty);
		}
	}

	BrowserProcessResponse^ JavascriptObjectWrapper::GetProperty(String^ memberName)
	{
		auto resp = _browserProcess->GetProperty(_object->Id, memberName);
        auto type = resp->Result->GetType();
        if (type == JavascriptObject::typeid) {
            auto obj = safe_cast<JavascriptObject^>(resp->Result);
            JavascriptPropertyWrapper^ propWrapper;
            bool exits = false;
            if (_wrappedProperties->TryGetValue(memberName, propWrapper)) {
                if (propWrapper->_javascriptObjectWrapper != nullptr) {
                    auto objWrapper = safe_cast<JavascriptObjectWrapper^>(propWrapper->_javascriptObjectWrapper);
                    exits = obj->Id == objWrapper->_object->Id;
                }
            }
            if (!exits) {
                auto jsObjectWrapper = gcnew JavascriptObjectWrapper(obj, _browserProcess);
                jsObjectWrapper->V8Value = propWrapper->V8Value.get();
                jsObjectWrapper->Bind();
                if (propWrapper->_javascriptObjectWrapper != nullptr)
                    delete propWrapper->_javascriptObjectWrapper;
                propWrapper->_javascriptObjectWrapper = jsObjectWrapper;
            }
            resp->Result = propWrapper->_javascriptObjectWrapper;
        }
        else if (type->IsArray && type->GetElementType() == JavascriptObject::typeid) {
            auto array = safe_cast<Array^>(resp->Result);
            CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(array->Length);
            for (int i = 0; i < array->Length; i++) {
                JavascriptObject^ jsObj = safe_cast<JavascriptObject^>(array->GetValue(i));
                if (jsObj != nullptr) {
                    auto objWrapper = gcnew JavascriptObjectWrapper(jsObj, _browserProcess);
                    objWrapper->Bind();
                    cefArray->SetValue(i, objWrapper->V8Value.get());
                }
                else
                    cefArray->SetValue(i, CefV8Value::CreateNull());
            }

            JavascriptPropertyWrapper^ propWrapper;
            if (_wrappedProperties->TryGetValue(memberName, propWrapper)) {
                if (propWrapper->_javascriptObjectWrapper != nullptr)
                    delete propWrapper->_javascriptObjectWrapper;
            }
            auto jsObjectWrapper = gcnew JavascriptObjectWrapper(nullptr, _browserProcess);
            jsObjectWrapper->V8Value = cefArray;
            resp->Result = propWrapper->_javascriptObjectWrapper = jsObjectWrapper;
        }

        return resp;
	};

	BrowserProcessResponse^ JavascriptObjectWrapper::SetProperty(String^ memberName, Object^ value)
	{
		return _browserProcess->SetProperty(_object->Id, memberName, value);
	};
}