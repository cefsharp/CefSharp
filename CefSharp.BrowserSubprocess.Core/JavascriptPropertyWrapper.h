// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptObjectWrapper.h"
#include "JavascriptPropertyHandler.h"

using namespace CefSharp::Internals;
using namespace System::Runtime::Serialization;

namespace CefSharp
{
    private ref class JavascriptPropertyWrapper : public IBindableJavascriptMember
    {
    private:
        MCefRefPtr<JavascriptPropertyHandler> _javascriptPropertyHandler;
        JavascriptObjectWrapper^ _owner;
        JavascriptProperty^ _prop;

    public:
        JavascriptPropertyWrapper(JavascriptProperty^ prop)
        {
            _prop = prop;
            _javascriptPropertyHandler = new JavascriptPropertyHandler(
                gcnew Func<Object^>(this, &JavascriptPropertyWrapper::GetProperty),
                gcnew Action<Object^>(this, &JavascriptPropertyWrapper::SetProperty)
            );
            JsObject = gcnew JavascriptObjectWrapper();
        }

        property JavascriptObjectWrapper^ JsObject
        {
            JavascriptObjectWrapper^ get() { return static_cast<JavascriptObjectWrapper^>(_prop->Value::get()); }
            void set(JavascriptObjectWrapper^ value) { _prop->Value::set(value); }
        }

        virtual void Bind(JavascriptObject^ owner)
        {
            _owner = static_cast<JavascriptObjectWrapper^>(owner);
            auto methodName = StringUtils::ToNative(_prop->JavascriptName);
            auto clrMethodName = _prop->JavascriptName;
            
            if(_prop->IsComplexType)
            {
                auto v8Value = _owner->V8Value->CreateObject(_javascriptPropertyHandler.get());

                JsObject->V8Value = v8Value;

                JsObject->Bind();

                _owner->V8Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);
            }
            else
            {
                _owner->V8Value->SetValue(methodName, V8_ACCESS_CONTROL_DEFAULT, V8_PROPERTY_ATTRIBUTE_NONE);
            }
        };

        void SetProperty(Object^ value);
        Object^ GetProperty();
    };
}