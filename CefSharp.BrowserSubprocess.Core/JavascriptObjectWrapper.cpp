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
		//Create property handler for get and set of Properties of this object
		_jsPropertyHandler = new JavascriptPropertyHandler(
			gcnew Func<String^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::GetProperty),
			gcnew Func<String^, Object^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::SetProperty)
			);

		//V8Value that represents this javascript object - only one per complex type
        auto javascriptObject = V8Value.get() ? V8Value->CreateObject(_jsPropertyHandler.get()) : CefV8Value::CreateObject(_jsPropertyHandler.get());
		auto objectName = StringUtils::ToNative(_object->JavascriptName);
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
        if (resp->Result->GetType() == JavascriptObject::typeid) {
            auto obj = safe_cast<JavascriptObject^>(resp->Result);
            JavascriptPropertyWrapper^ p;
            bool exits = false;
            if (_wrappedProperties->TryGetValue(memberName, p)) {
                if (p->_javascriptObjectWrapper != nullptr) {
                    auto w = safe_cast<JavascriptObjectWrapper^>(p->_javascriptObjectWrapper);
                    exits = obj->Id == w->_object->Id;
                }
            }
            if (!exits) {
                auto j = gcnew JavascriptObjectWrapper(obj, _browserProcess);
                j->V8Value = p->V8Value.get();
                j->Bind();
                if (p->_javascriptObjectWrapper != nullptr)
                    delete p->_javascriptObjectWrapper;
                p->_javascriptObjectWrapper = j;
            }
            resp->Result = p->_javascriptObjectWrapper;
        }
        return resp;
	};

	BrowserProcessResponse^ JavascriptObjectWrapper::SetProperty(String^ memberName, Object^ value)
	{
		return _browserProcess->SetProperty(_object->Id, memberName, value);
	};
}