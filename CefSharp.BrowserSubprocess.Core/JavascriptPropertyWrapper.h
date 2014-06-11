// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptObjectWrapper.h"
#include "JavascriptPropertyHandler.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    [System::Runtime::Serialization::DataContractAttribute]
    private ref class JavascriptPropertyWrapper : public JavascriptProperty
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
            Value = gcnew JavascriptObjectWrapper();
        }
        
        property JavascriptObjectWrapper^ Value
        {
            JavascriptObjectWrapper^ get() { return static_cast<JavascriptObjectWrapper^>(JavascriptProperty::Value::get()); }
            void set(JavascriptObjectWrapper^ value) { JavascriptProperty::Value::set(value); }
        }

        virtual void Bind(JavascriptObject^ owner) override
        {
            _owner = static_cast<JavascriptObjectWrapper^>(owner);
            auto v8Value = _owner->Value->CreateObject(_javascriptPropertyHandler.get());
            
            Value->Value = v8Value;

            _owner->Value->SetValue(StringUtils::ToNative(Description->JavascriptName), v8Value, V8_PROPERTY_ATTRIBUTE_NONE);

            Value->Bind();
        };

        void Clone(JavascriptProperty^ obj)
        {
            Description = obj->Description;

            Value->Clone( obj->Value );
        }

        void SetProperty(Object^ value);
        Object^ GetProperty();
    };
}