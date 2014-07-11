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
    [DataContract]
    private ref class JavascriptPropertyWrapper : public JavascriptProperty, IBindableJavascriptMember
    {
    private:
        MCefRefPtr<JavascriptPropertyHandler> _javascriptPropertyHandler;
        JavascriptObjectWrapper^ _owner;

    public:

        JavascriptPropertyWrapper()
        {
            _javascriptPropertyHandler = new JavascriptPropertyHandler(
                gcnew Func<Object^>(this, &JavascriptPropertyWrapper::GetProperty),
                gcnew Action<Object^>(this, &JavascriptPropertyWrapper::SetProperty)
            );
            JsObject = gcnew JavascriptObjectWrapper();
        }

        property JavascriptObjectWrapper^ JsObject
        {
            JavascriptObjectWrapper^ get() { return static_cast<JavascriptObjectWrapper^>(JavascriptProperty::Value::get()); }
            void set(JavascriptObjectWrapper^ value) { JavascriptProperty::Value::set(value); }
        }

        virtual void Bind(JavascriptObject^ owner, bool topLevel)
        {
            _owner = static_cast<JavascriptObjectWrapper^>(owner);
            auto methodName = StringUtils::ToNative(JavascriptName);
            CefRefPtr<CefV8Value> topLevelParent = nullptr;
            
            if(topLevel)
            {
                topLevelParent = _owner->V8Value->CreateObject(_javascriptPropertyHandler.get());
                JsObject->V8Value = topLevelParent;
            }

            JsObject->Bind(false);

            if(topLevel)
            {
                _owner->V8Value->SetValue(methodName, topLevelParent, V8_PROPERTY_ATTRIBUTE_NONE);
            }
            else
            {
                _owner->V8Value->SetValue(methodName, V8_ACCESS_CONTROL_DEFAULT, V8_PROPERTY_ATTRIBUTE_NONE);
            }
        };

        void Clone(JavascriptProperty^ obj)
        {
            JavascriptName = obj->JavascriptName;
            ManagedName = obj->ManagedName;

            JsObject->Clone(obj->Value);
        }

        void SetProperty(Object^ value);
        Object^ GetProperty();
    };
}