// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"
#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
	void JavascriptObjectWrapper::Bind()
	{
		//Create property handler with only a getter for root objects
		//TODO: Store reference to this
		auto propertyHandler = new JavascriptPropertyHandler(
			gcnew Func<String^, Object^>(this, &JavascriptObjectWrapper::GetProperty),
			gcnew Action<String^, Object^>(this, &JavascriptObjectWrapper::SetProperty)
			);

		auto v8Value = V8Value->CreateObject(propertyHandler);
		auto objectName = StringUtils::ToNative(_object->JavascriptName);
		V8Value->SetValue(objectName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);

		for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(_object->Methods))
		{
			auto wrappedMethod = gcnew JavascriptMethodWrapper(method, _object->Id);
			wrappedMethod->V8Value = v8Value;
			wrappedMethod->Bind();

			_wrappedMethods->Add(wrappedMethod);
		}

		for each (JavascriptProperty^ prop in Enumerable::OfType<JavascriptProperty^>(_object->Properties))
		{
			auto wrappedproperty = gcnew JavascriptPropertyWrapper(prop, _object->Id);
			wrappedproperty->V8Value = v8Value;
			wrappedproperty->Bind();

			_wrappedProperties->Add(wrappedproperty);
		}
	}

	Object^ JavascriptObjectWrapper::GetProperty(String^ memberName)
	{
		auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

		return browserProxy->GetProperty(_object->Id, memberName);
	};

	void JavascriptObjectWrapper::SetProperty(String^ memberName, Object^ value)
	{
		auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

		browserProxy->SetProperty(_object->Id, memberName, value);
	};
}