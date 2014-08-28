// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptMethodWrapper.h"
#include "JavascriptPropertyWrapper.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptObjectWrapper
    {
    private:
        JavascriptObject^ _object;
        List<JavascriptMethodWrapper^>^ _wrappedMethods;
        List<JavascriptPropertyWrapper^>^ _wrappedProperties;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptObjectWrapper(JavascriptObject^ object)
        {
            _object = object;

            _wrappedMethods = gcnew List<JavascriptMethodWrapper^>();
            _wrappedProperties = gcnew List<JavascriptPropertyWrapper^>();
        }

        void Bind()
        {
            //Create property handler with only a getter for root objects
            auto propertyHandler = new JavascriptPropertyHandler(
                gcnew Func<String^, Object^>(this, &JavascriptObjectWrapper::GetProperty),
                nullptr
                );

            auto v8Value = V8Value->CreateObject(propertyHandler);
            auto methodName = StringUtils::ToNative(_object->JavascriptName);
            V8Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);

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
                wrappedproperty->V8Value = V8Value.get();
                wrappedproperty->Bind();

                _wrappedProperties->Add(wrappedproperty);
            }
        }

        Object^ GetProperty(String^ memberName);
    };
}